
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientApp.Services;

public class HubService
{
    private readonly AuthTokenService  _authTokenService;

    public HubService(AuthTokenService authTokenService)
    {
        _authTokenService = authTokenService;
    }

    public HubConnection Build()
    {
        return new HubConnectionBuilder()
            .WithUrl("https://gift-application-service-garfg.ondigitalocean.app/apphub", options => 
            {
                options.AccessTokenProvider = async () => await _authTokenService.RefreshTokenAsync();
            })
            .Build();
    }
}