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

            var lobbyState = lobbyService.Lobbies.First(x => x.Id == lobbyId);
            
            lobbyState.Players?.Add(player);
            lobbyState.PlayerCount = lobbyState.Players!.Count;
            
            lobbyService.TriggerLobbyPageRefresh();
        });
        
        _hubConnection.On<int, PlayerModel>("onPlayerDisconnected", (lobbyId, player) =>
        {
            if (0 >= lobbyId || lobbyId >= lobbyService.Lobbies.Count)
            {
                return;
            }

            var lobbyState = lobbyService.Lobbies.First(x => x.Id == lobbyId);
            
            lobbyState.Players?.RemoveAll(x => string.Equals(x.Name.Replace(' ', '_'), player.Name.Replace(' ', '_'), StringComparison.InvariantCultureIgnoreCase));
            lobbyState.PlayerCount = lobbyState.Players!.Count;
            
            lobbyService.TriggerLobbyPageRefresh();
        });
        
        _hubConnection.On<int, object>("onBeatmapChanged", async (lobbyId, _) =>
        {
            if (0 >= lobbyId || lobbyId >= lobbyService.Lobbies.Count)
            {
                return;
            }

            // Insanely (!) stupid workaround because I can't bother fixing it properly
            // in the bot. :-)
            await Task.Delay(500);
            
            await lobbyService.GetLobbyExtended(lobbyId);
            lobbyService.TriggerLobbyPageRefresh();
        });
        
        _hubConnection.On<int, object>("onSettingsUpdated", async (lobbyId, _) =>
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