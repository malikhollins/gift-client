using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Extra;

public partial class ManualPairingEditor
{
    [Parameter] public int HouseId { get; set; }
    [Parameter] public IReadOnlyList<UserList> ActiveLists { get; set; } = Array.Empty<UserList>();
    [Parameter] public bool IsBlocked { get; set; }
    [Parameter] public Func<int, string> GetUserName { get; set; } = null!;
    [Parameter] public Func<int, string> GetListName { get; set; } = null!;
    [Parameter] public EventCallback<bool> OnBusyChanged { get; set; }
    [Parameter] public EventCallback<List<PairingData>> OnPairingsUploaded { get; set; }

    [Inject] private PairService PairService { get; set; } = null!;

    private readonly List<PairingData> _manualPairings = new();
    private bool _isUploading;
    private bool _hasError;
    private string? _message;

    private bool IsBusy => IsBlocked || _isUploading;

    private bool CanUploadManualPairings
    {
        get
        {
            if (_manualPairings.Count == 0)
            {
                return false;
            }

            var selectedListIds = _manualPairings.Select(pairing => pairing.ListId).ToHashSet();
            var activeListIds = ActiveLists.Select(list => list.ListId).ToHashSet();

            return selectedListIds.IsSubsetOf(activeListIds);
        }
    }

    private void AddManualPairing()
    {
        var list = ActiveLists.FirstOrDefault();
        if (list is not null)
        {
            _manualPairings.Add(new PairingData { UserId = list.Owner, ListId = list.ListId });
        }
    }

    private void RemoveManualPairing(PairingData pairing)
    {
        _manualPairings.Remove(pairing);
    }

    private async Task UploadManualPairingsAsync()
    {
        if (!CanUploadManualPairings)
        {
            return;
        }

        _isUploading = true;
        await OnBusyChanged.InvokeAsync(true);
        _message = null;
        _hasError = false;

        try
        {
            var response = await PairService.SetManualPairingsAsync(HouseId, _manualPairings);
            _message = response.IsSuccessStatusCode
                ? "Manual pairings uploaded successfully."
                : "Manual pairings could not be uploaded. Check the selected lists and try again.";
            _hasError = !response.IsSuccessStatusCode;

            if (response.IsSuccessStatusCode)
            {
                await OnPairingsUploaded.InvokeAsync(_manualPairings
                    .Select(pairing => new PairingData
                    {
                        UserId = pairing.UserId,
                        ListId = pairing.ListId
                    })
                    .ToList());
            }
        }
        catch
        {
            _message = "Manual pairings could not be uploaded. Check your connection and try again.";
            _hasError = true;
        }
        finally
        {
            _isUploading = false;
            await OnBusyChanged.InvokeAsync(false);
        }
    }
}
