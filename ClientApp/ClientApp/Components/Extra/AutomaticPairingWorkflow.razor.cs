using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientApp.Components.Extra;

public partial class AutomaticPairingWorkflow : IAsyncDisposable
{
    [Parameter] public int HouseId { get; set; }
    [Parameter] public House House { get; set; } = null!;
    [Parameter] public IReadOnlyList<UserList> ActiveLists { get; set; } = Array.Empty<UserList>();
    [Parameter] public int RefreshVersion { get; set; }
    [Parameter] public bool IsBlocked { get; set; }
    [Parameter] public EventCallback<bool> OnBusyChanged { get; set; }

    [Inject] private PairService PairService { get; set; } = null!;
    [Inject] private UserInfoService UserInfoService { get; set; } = null!;
    [Inject] private HubService HubService { get; set; } = null!;

    private readonly List<PairingData> _pairings = new();
    private List<PairingRepairEntry> _repairEntries = new();
    private HubConnection? _pairingHubConnection;
    private IDisposable? _pairingNotificationSubscription;
    private bool _isGenerating;
    private bool _isApplying;
    private bool _isRepairing;
    private bool _hasError;
    private string? _message;
    private int _lastRefreshVersion = -1;
    private bool _hasLoaded;

    private bool IsBusy => IsBlocked || _isGenerating || _isApplying || _isRepairing;
    private bool _canSubmitRepairs => _repairEntries.Count > 0 && _repairEntries.All(entry => entry.ListId > 0);

    protected override async Task OnParametersSetAsync()
    {
        if (House is null)
        {
            return;
        }

        if (!_hasLoaded)
        {
            _hasLoaded = true;
            _lastRefreshVersion = RefreshVersion;
            await RefreshPairingStateAsync();
            await RegisterPairingNotificationsAsync();
        }
        else if (_lastRefreshVersion != RefreshVersion)
        {
            _lastRefreshVersion = RefreshVersion;
            await RefreshPairingStateAsync();
        }
    }

    private async Task RegisterPairingNotificationsAsync()
    {
        var user = UserInfoService.GetUserInfo();
        if (user is null)
        {
            return;
        }

        _pairingHubConnection ??= HubService.Build();
        _pairingNotificationSubscription ??= _pairingHubConnection.On<HousePairingUpdateNotification>("NotifyPairingsNeedUpdate", async notification =>
        {
            if (notification.HouseId == HouseId)
            {
                await InvokeAsync(async () =>
                {
                    await RefreshPairingStateAsync();
                    StateHasChanged();
                });
            }
        });

        if (_pairingHubConnection.State == HubConnectionState.Disconnected)
        {
            await _pairingHubConnection.StartAsync();
            await _pairingHubConnection.SendAsync("JoinHouses", user.Id, new List<House> { House });
        }
    }

    private async Task RefreshPairingStateAsync()
    {
        var selectedListIds = _repairEntries.ToDictionary(entry => entry.UserId, entry => entry.ListId);
        var pairings = await PairService.GetAllPairingsAsync(HouseId);
        var missingUserIds = await PairService.GetPairingStatusAsync(HouseId);

        _pairings.Clear();
        _pairings.AddRange(pairings);
        _repairEntries = missingUserIds
            .Distinct()
            .Select(userId => new PairingRepairEntry
            {
                UserId = userId,
                ListId = selectedListIds.TryGetValue(userId, out var selectedListId)
                    && ActiveLists.Any(list => list.ListId == selectedListId)
                    ? selectedListId
                    : 0,
                UserName = GetUserName(userId)
            })
            .ToList();

        if (_repairEntries.Count > 0)
        {
            _message = "Pairings need attention. Repair the missing assignments to continue.";
            _hasError = true;
        }
        else
        {
            _message = _pairings.Count > 0 ? "Active pairings are up to date." : _message;
            _hasError = false;
        }
    }

    private void UpdateRepairList(int userId, int listId)
    {
        var repairEntry = _repairEntries.FirstOrDefault(entry => entry.UserId == userId);
        if (repairEntry is null)
        {
            return;
        }

        repairEntry.ListId = listId;
        if (listId <= 0)
        {
            _message = "Select a list for each missing member before saving.";
            _hasError = true;
        }
    }

    private int ParseListId(object? value)
    {
        return int.TryParse(value?.ToString(), out var listId) ? listId : 0;
    }

    private IEnumerable<UserList> GetAvailableRepairListsForUser(int userId)
    {
        var usedListIds = _repairEntries
            .Where(entry => entry.UserId != userId && entry.ListId > 0)
            .Select(entry => entry.ListId)
            .ToHashSet();

        return ActiveLists
            .Where(list => !usedListIds.Contains(list.ListId))
            .OrderBy(list => list.OwnerName ?? string.Empty)
            .ThenBy(list => list.ListId);
    }

    private async Task GenerateAsync()
    {
        await SetBusyAsync(true);
        _isGenerating = true;
        _message = null;
        _hasError = false;

        try
        {
            _pairings.Clear();
            _pairings.AddRange(await PairService.GenerateRandomPairingsAsync(HouseId));
            _message = _pairings.Count > 0
                ? "Review the assignments, then apply them when ready."
                : "No pairings were generated. This household may need more lists.";
            _hasError = _pairings.Count == 0;
        }
        catch
        {
            _pairings.Clear();
            _message = "Pairings could not be generated. This household may need more lists.";
            _hasError = true;
        }
        finally
        {
            _isGenerating = false;
            await SetBusyAsync(false);
        }
    }

    private async Task ApplyAsync()
    {
        await SetBusyAsync(true);
        _isApplying = true;
        _message = null;
        _hasError = false;

        try
        {
            var response = await PairService.ApplyPairingsAsync(HouseId);
            _message = response.IsSuccessStatusCode
                ? "Pairings applied successfully."
                : "Pairings could not be applied. Generate a new preview and try again.";
            _hasError = !response.IsSuccessStatusCode;
            if (response.IsSuccessStatusCode)
            {
                await RefreshPairingStateAsync();
            }
        }
        catch
        {
            _message = "Pairings could not be applied. Check your connection and try again.";
            _hasError = true;
        }
        finally
        {
            _isApplying = false;
            await SetBusyAsync(false);
        }
    }

    private async Task DeleteActivePairing()
    {
        await SetBusyAsync(true);
        _isApplying = true;
        _message = null;
        _hasError = false;

        try
        {
            var response = await PairService.DeleteActivePairing(HouseId);
            if (response.IsSuccessStatusCode)
            {
                _pairings.Clear();
                _repairEntries.Clear();
                _message = "The active pairing event was stopped.";
                await RefreshPairingStateAsync();
            }
            else
            {
                _message = "The active pairing event could not be stopped. Try again in a moment.";
                _hasError = true;
            }
        }
        catch
        {
            _message = "The pairing event could not be stopped. Check your connection and try again.";
            _hasError = true;
        }
        finally
        {
            _isApplying = false;
            await SetBusyAsync(false);
        }
    }

    private async Task RepairMissingPairingsAsync()
    {
        if (!_canSubmitRepairs)
        {
            return;
        }

        await SetBusyAsync(true);
        _isRepairing = true;
        _message = null;
        _hasError = false;
        var successfulRepairs = 0;
        var failedRepairs = 0;

        try
        {
            foreach (var repairEntry in _repairEntries)
            {
                try
                {
                    var response = await PairService.SetManualPairAsync(HouseId, new PairingData
                    {
                        UserId = repairEntry.UserId,
                        ListId = repairEntry.ListId
                    });
                    if (response.IsSuccessStatusCode) successfulRepairs++;
                    else failedRepairs++;
                }
                catch
                {
                    failedRepairs++;
                }
            }

            await RefreshPairingStateAsync();
            if (failedRepairs == 0)
            {
                _message = "Missing pairings were repaired successfully.";
                _hasError = false;
            }
            else if (successfulRepairs > 0)
            {
                _message = $"{successfulRepairs} repair(s) saved, but {failedRepairs} could not be saved. Review the remaining repairs and try again.";
                _hasError = true;
            }
            else
            {
                _message = "Repairs could not be saved. Check your connection and try again.";
                _hasError = true;
            }
        }
        catch
        {
            await RefreshPairingStateAsync();
            _message = "Repairs could not be saved. Check your connection and try again.";
            _hasError = true;
        }
        finally
        {
            _isRepairing = false;
            await SetBusyAsync(false);
        }
    }

    private string GetUserName(int userId)
    {
        return House.Lists.FirstOrDefault(list => list.Owner == userId)?.OwnerName ?? $"User {userId}";
    }

    private string GetListName(int listId)
    {
        var list = House.Lists.FirstOrDefault(item => item.ListId == listId);
        return list is null ? $"List {listId}" : $"{list.OwnerName}'s list";
    }

    private async Task SetBusyAsync(bool busy)
    {
        await OnBusyChanged.InvokeAsync(busy);
    }

    public async ValueTask DisposeAsync()
    {
        _pairingNotificationSubscription?.Dispose();
        if (_pairingHubConnection is not null)
        {
            await _pairingHubConnection.DisposeAsync();
        }
    }

    private sealed class HousePairingUpdateNotification
    {
        public int HouseId { get; set; }
    }
}
