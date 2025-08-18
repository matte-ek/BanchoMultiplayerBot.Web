using BanchoMultiplayerBot.Web.State;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Services;

public class HealthService(
    ApiService apiService,
    ISnackbar snackbar)
{
    public HealthState State { get; } = new();

    public async Task GetState()
    {
        var request = await apiService.Get<HealthState?>("api/health");
        if (request == null)
        {
            snackbar.Add("Failed to get health state", Severity.Error);
            return;
        }
        
        State.IsBanchoConnected = request.IsBanchoConnected;
        State.HasConfigurationError = request.HasConfigurationError;
        State.IsLobbyActive = request.IsLobbyActive;
    }
}