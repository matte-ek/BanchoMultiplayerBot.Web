using BanchoMultiplayerBot.Web.DataTransferObjects;
using BanchoMultiplayerBot.Web.Services;
using BanchoMultiplayerBot.Web.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Pages;

public partial class Lobby(LobbyService lobbyService, MessageService messageService, IJSRuntime jsRuntime) : ComponentBase
{
    [Parameter]
    public int LobbyId { get; set; }

    private LobbyState State { get; set; } = null!;
    private List<ReadMessage> Messages => messageService.Messages;
    private string ChatMessage { get; set; } = string.Empty;

    private bool _isRequestingLobbyData = true;
    private bool _isRequestingMessages = true;

    private ElementReference _chatBox;

    protected override async Task OnInitializedAsync()
    {
        LobbyService.OnLobbyUpdated += async () =>
        {
            StateHasChanged();
            await jsRuntime.InvokeVoidAsync("chatbox.scrollToBottomIfNearEnd", _chatBox);
        };
        
        await base.OnInitializedAsync();
    }
    
    protected override async Task OnParametersSetAsync()
    {
        _isRequestingLobbyData = true;
        _isRequestingMessages = true;
        
        State = lobbyService.Lobbies.First(x => x.Id == LobbyId);

        _ = lobbyService.GetLobbyExtended(LobbyId).ContinueWith(e =>
        {
            _isRequestingLobbyData = false;
            StateHasChanged();
        });

        _ = messageService.GetMessages(LobbyId).ContinueWith(async _ =>
        {
            _isRequestingMessages = false;
            StateHasChanged();
            await jsRuntime.InvokeVoidAsync("chatbox.scrollToBottom", _chatBox);
        });
        
        await base.OnParametersSetAsync();
    }
    
    private async Task OnChatboxKeyPress(KeyboardEventArgs args)
    {
        if (args.Key != "Enter" || string.IsNullOrEmpty(ChatMessage))
        {
            return;
        }

        await messageService.Send(LobbyId, ChatMessage);
        
        ChatMessage = string.Empty;
        
        StateHasChanged();
    }

    private static Color GetMessageColor(ReadMessage message)
    {
        if (message.IsAdministratorMessage)
        {
            return Color.Secondary;
        }

        if (message.IsBanchoMessage)
        {
            return Color.Info;
        }

        return Color.Default;
    }
}