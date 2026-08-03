using ClientApp.Services;
using ClientApp.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace ClientApp.Components.Pages
{
    public partial class Login
    {
        [Inject] LoginService LoginService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] UserInfoService UserInfoService { get; set; } = null!;
        [Inject] ILogger<Login> Logger { get; set; } = null!;

        private BlazorBootstrap.Button _loginButton = null!;
        private bool _retry = false;

        protected override async Task OnInitializedAsync()
        {
            await LoginAsync();
            await base.OnInitializedAsync();
        }

        public async Task LoginAsync()
        {
            if (_loginButton?.Loading ?? false)
            {
                return;
            }

            _loginButton?.ShowLoading();

            try
            {
                Logger.LogInformation("Started login process.");
                bool loggedIn = await LoginService.LoginAsync();
                if (loggedIn)
                {
                    var userInfo = UserInfoService.GetUserInfo();
                    if (userInfo!.Name.IsNullOrEmpty())
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
                _retry = true;
            }
            finally
            {
                _loginButton?.HideLoading();
            }
        }
    }
}
