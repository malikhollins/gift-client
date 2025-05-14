using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ClientApp.Components.Pages
{
    public partial class LoginGiftAll
    {
        [Inject] LoginService LoginService { get; set; } = null!;

        private bool _isLoggingIn = false;
        public async void Login()
        {
            if (_isLoggingIn)
            {
                return;
            }

            try
            {
                _isLoggingIn = true;

                await LoginService.LoginAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _isLoggingIn = false;
            }
        }
    }
}
