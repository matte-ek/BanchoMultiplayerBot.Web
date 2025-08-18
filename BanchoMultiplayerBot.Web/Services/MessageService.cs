using BanchoMultiplayerBot.Web.DataTransferObjects;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Services;

public class MessageService(ApiService apiService, ISnackbar snackbar)
{
    public List<ReadMessage> Messages = [];

    public async Task GetMessages(int lobbyId)
    {
        var response = await apiService.Get<ReadMessage[]?>($"api/message/lobby/{lobbyId}");

        if (response == null)
        {
            snackbar.Add($"Unable to get messages for lobby {lobbyId}", Severity.Error);
            return;
        }

        Messages = response.ToList();
    }
}