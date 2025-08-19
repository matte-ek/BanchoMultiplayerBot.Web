using BanchoMultiplayerBot.Web.DataTransferObjects;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Services;

public class MessageService(ApiService apiService, ISnackbar snackbar)
{
    public List<ReadMessage> Messages = [];
    public int LobbyId { get; private set; }

    public async Task GetMessages(int lobbyId)
    {
        LobbyId = lobbyId;
        
        var response = await apiService.Get<ReadMessage[]?>($"api/message/lobby/{lobbyId}?offset=0&limit=500");

        if (response == null)
        {
            snackbar.Add($"Unable to get messages for lobby {lobbyId}", Severity.Error);
            return;
        }

        Messages = response.ToList();
    }

    public async Task Send(int lobbyId, string message)
    {
        await apiService.Post("api/message/lobby/send", new WriteMessage(lobbyId, message));
    }
}