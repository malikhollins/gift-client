using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using ClientApp.Models;
using BlazorBootstrap;

namespace ClientApp.Components.Extra
{
    public partial class EmailList
    {
        [Inject] UserService UserService { get; set; } = null!;

        private string _inputString { get; set; } = string.Empty;

        private List<User> _invitedUsers = new List<User>();

        [Parameter]
        public EventCallback<User> OnUserInvited { get; set; }


        private void OnAutoCompleteChanged(User user)
        {
            if (user == null)
            {
                return;
            }

            _invitedUsers.Add(user);
            OnUserInvited.InvokeAsync( user );
        }

        public async Task<AutoCompleteDataProviderResult<User>> UsersDataProvider(AutoCompleteDataProviderRequest<User> request)
        {
            var users = await GetPossibleUsersAsync( request.Filter.Value, request.CancellationToken );
            users.RemoveAll(_invitedUsers.Contains);
            return await Task.FromResult(new AutoCompleteDataProviderResult<User>
            {
                Data = users,
                TotalCount = users.Count,
            });
        }

        public async Task<List<User>> GetPossibleUsersAsync(string input, CancellationToken cancellationToken)
            => await UserService.BulkGetUsersAsync(input, cancellationToken );
    }
}
