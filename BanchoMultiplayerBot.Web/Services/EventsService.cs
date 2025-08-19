using BanchoMultiplayerBot.Web.Data;
using BanchoMultiplayerBot.Web.DataTransferObjects;
using BanchoMultiplayerBot.Web.Pages;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Services;

public class EventsService(AppConfiguration appConfiguration, MessageService messageService, LobbyService lobbyService, ISnackbar snackbar)
{
    private readonly HubConnection _hubConnection = new HubConnectionBuilder()
        .WithUrl(appConfiguration.BackendUri + "hubs/lobby", options =>
        {
            options.HttpMessageHandlerFactory = _ => new CookieAuthenticationHandler();
        })
        .WithAutomaticReconnect()
        .Build();

    private bool _registeredEvents;
    
    public async Task Connect()
    {
        if (!_registeredEvents)
        {
            RegisterEventHandlers();
            _registeredEvents = true;
        }
        
        await _hubConnection.StartAsync();
    }

    public async Task Disconnect() => await _hubConnection.StopAsync();

    private void RegisterEventHandlers()
    {
        _hubConnection.On<int, ReadMessage>("onMessage", (lobbyId, message) =>
        {
            if (messageService.LobbyId != lobbyId)
            {
                return;
            }
            
            messageService.Messages.Add(message);
            lobbyService.TriggerLobbyPageRefresh();
        });

        _hubConnection.On<int, PlayerModel>("onPlayerJoined", (lobbyId, player) =>
        {
            if (0 >= lobbyId || lobbyId >= lobbyService.Lobbies.Count)
            {
                return;
            }
            
            lobbyService.Lobbies[lobbyId].Players?.Add(player);
            lobbyService.TriggerLobbyPageRefresh();
        });
        
        _hubConnection.On<int, PlayerModel>("onPlayerDisconnected", (lobbyId, player) =>
        {
            if (0 >= lobbyId || lobbyId >= lobbyService.Lobbies.Count)
            {
                return;
            }
            
            lobbyService.Lobbies[lobbyId].Players?.RemoveAll(x => x.Name == player.Name);
            lobbyService.TriggerLobbyPageRefresh();
        });
        
        _hubConnection.On<int>("onBeatmapChanged", async lobbyId =>
        {
            if (0 >= lobbyId || lobbyId >= lobbyService.Lobbies.Count)
            {
                return;
            }

            await lobbyService.GetLobbyExtended(lobbyId);
            lobbyService.TriggerLobbyPageRefresh();
        });
    }
}