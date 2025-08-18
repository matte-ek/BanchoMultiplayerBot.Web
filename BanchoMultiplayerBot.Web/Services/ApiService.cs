using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Services;

public class ApiService(HttpClient client, AppConfiguration appConfiguration, ISnackbar snackbar)
{
    public async Task<bool> Get(string endpoint) => await SendRequest(HttpMethod.Get, endpoint);
    public async Task<T?> Get<T>(string endpoint) => await SendAndParseRequest<object, T?>(HttpMethod.Get, endpoint, null);
    
    public async Task<bool> Post(string endpoint) => await SendRequest(HttpMethod.Post, endpoint);
    public async Task<bool> Post<TIn>(string endpoint, TIn data) => (await SendRequest(HttpMethod.Post, endpoint, data))?.IsSuccessStatusCode == true;
    public async Task<TOut?> Post<TIn, TOut>(string endpoint, TIn data) => await SendAndParseRequest<TIn, TOut>(HttpMethod.Post, endpoint, data);
    
    private async Task<TOut?> SendAndParseRequest<TIn, TOut>(HttpMethod method, string endpoint, TIn? data)
    {
        var request = await SendRequest(method, endpoint, data);

        if (request == null)
        {
            return default;
        }
        
        return await request.Content.ReadFromJsonAsync<TOut>();
    }
    
    private async Task<HttpResponseMessage?> SendRequest<T>(HttpMethod method, string endpoint, T? body)
    {
        var message = new HttpRequestMessage(method, appConfiguration.BackendUri + endpoint);

        if (body != null)
        {
            message.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        }
        
        var response = await client.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            if ((int)response.StatusCode >= 500)
            {
                // Other errors will be shown by the service doing the request.
                snackbar.Add($"Failed to send request to endpoint {endpoint}, status code: {response.StatusCode}", Severity.Error);
            }

            return null;
        }
        
        return response;
    }

    private async Task<bool> SendRequest(HttpMethod method, string endpoint)
    {
        var response =  await SendRequest<object>(method, endpoint, null);
        return response is { IsSuccessStatusCode: true };
    }
}