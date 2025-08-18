using BanchoMultiplayerBot.Web;
using BanchoMultiplayerBot.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("configuration.json");

builder.Services.AddMudServices();
builder.Services.AddScoped(_ => new HttpClient(new CookieAuthenticationHandler()));

builder.Services
    .AddScoped<AppConfiguration>()
    .AddScoped<ApiService>()
    .AddScoped<AuthService>()
    .AddScoped<HealthService>()
    .AddScoped<MessageService>()
    .AddScoped<BehaviorService>()
    .AddScoped<LobbyService>();

await builder.Build().RunAsync();