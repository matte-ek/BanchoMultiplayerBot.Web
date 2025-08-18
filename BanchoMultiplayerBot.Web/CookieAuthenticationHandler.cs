using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace BanchoMultiplayerBot.Web;

public class CookieAuthenticationHandler : DelegatingHandler
{
    public CookieAuthenticationHandler()
    {
        InnerHandler = new HttpClientHandler();
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}