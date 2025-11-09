using BlazorBootstrap;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class Login
    {
        [Inject] LoginService LoginService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        private BlazorBootstrap.Button _loginButton = null!;

        public async void OnLoginClickedAsync()
        {
            if (_loginButton.Loading)
            {
                return;
            }

            try
            {
                _loginButton.ShowLoading();
                bool loggedIn = await LoginService.LoginAsync();
                if (loggedIn)
                {
                    NavigationManager.NavigateTo("/homepage");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _loginButton.HideLoading();
            }
        }
    }
}
