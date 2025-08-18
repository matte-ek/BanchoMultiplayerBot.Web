using BanchoMultiplayerBot.Web.DataTransferObjects;
using BanchoMultiplayerBot.Web.State;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Services;

public class AuthService(
    ApiService apiService,
    AppConfiguration appConfiguration,
    ISnackbar snackbar,
    NavigationManager navigationManager)
{
    public AuthenticationState State { get; } = new();

    public async Task Validate()
    {
        var validation = await apiService.Get<ReadAuthValidation?>("api/auth/validate");
        
        if (validation == null)
        {
            State.IsAuthenticated = false;
            return;
        }

        if (!int.TryParse(validation.Value.Id, out var id))
        {
            snackbar.Add("Received invalid osu! id during validation.", Severity.Error);   
            State.IsAuthenticated = false;
            return;
        }
        
        State.IsAuthenticated = true;
        State.OsuId = id;
        State.OsuUsername = validation.Value.Username;
    }

    public void Authenticate()
    {
        navigationManager.NavigateTo(appConfiguration.BackendUri + "api/auth/osu");
    }
}