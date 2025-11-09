using Auth0.OidcClient;
using ClientApp.Services;

namespace ClientApp
{
    public static class Auth0Program
    {
        public static void SetupAuth0Authentication( this IServiceCollection services )
        {
            services.AddSingleton(new Auth0Client(new Auth0ClientOptions
            {
                Domain = "dev-783iaokvar74ul5a.us.auth0.com", // these strings are not confiedntial
                ClientId = "jmnIEMYDbuiLUUsKPgYIruQg4op2DHsZ",
                Scope = "openid profile email offline_access",
                RedirectUri = "myapp://callback/",
                PostLogoutRedirectUri = "myapp://callback/",
            }));

            services.AddAuthorizationCore();
            //services.AddScoped<AuthenticationStateProvider, Auth0AuthenticationStateProvider>();
            services.AddSingleton<LoginService>();
            services.AddSingleton<AuthTokenStorage>();
            services.AddSingleton<AuthTokenService>();
        }
    }
}
