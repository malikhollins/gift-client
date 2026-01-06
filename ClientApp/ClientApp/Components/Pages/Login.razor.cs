using ClientApp.Services;
using ClientApp.Utils;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class Login
    {
        [Inject] LoginService LoginService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] UserInfoService UserInfoService { get; set; } = null!;

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
                    var userInfo = UserInfoService.GetUserInfo();
                    if (userInfo.Name.IsNullOrEmpty())
                    {
                        NavigationManager.NavigateTo("/profile");
                    }
                    else
                    {
                        NavigationManager.NavigateTo("/homepage");
                    }
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
