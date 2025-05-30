using ClientApp.Services;
using Microsoft.Extensions.Logging;

namespace ClientApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddBlazorBootstrap();
            builder.Services.AddMauiBlazorWebView(); 
            builder.Services.AddHttpClient();
            
            // setup web services
            builder.Services.AddMauiBlazorWebView();

            // setup default http client
            builder.Services.AddHttpClient( "base-url", ( client ) => 
            {
                client.BaseAddress = new Uri( "https://localhost" );
            } );

            // setup auth0 login service
            builder.Services.SetupAuth0Authentication();

            // setup app specific services
            builder.Services.AddSingleton<HouseService>();
            builder.Services.AddSingleton<UserInfoService>();
#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
