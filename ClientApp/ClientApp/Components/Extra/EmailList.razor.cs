using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using ClientApp.Models;
using ClientApp.Utils;

namespace ClientApp.Components.Extra
{
    public partial class EmailList
    {
        [Inject] UserService UserService { get; set; } = null!;

        private string _inputString { get; set; } = string.Empty;

        private string _searchingString { get; set; } = string.Empty;

        private CancellationTokenSource? _cts { get; set; }

        private List<User> _possibleUsers { get; set; } = new List<User>();

        [Parameter]
        public EventCallback<User> OnUserInvited { get; set; }

        public void OnInputChanged()
        {
            if (_inputString.Equals(_searchingString))
            {
                return;
            }

            _cts?.Cancel();
            GetPossibleUsersAsync( _inputString ).SafeFireAndForget();
        }

        public async Task GetPossibleUsersAsync( string input, CancellationToken ct = default )
        {
            var users = await UserService.BulkGetUsersAsync(input);
            if ( ct.IsCancellationRequested)
            {
                return;
            }
            _possibleUsers = users;
        }

        public Task InviteUser( User user ) =>  OnUserInvited.InvokeAsync(user);
    }
}
