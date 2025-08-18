using BanchoMultiplayerBot.Web.Data;
using BanchoMultiplayerBot.Web.DataTransferObjects;
using BanchoMultiplayerBot.Web.State;
using MudBlazor;

namespace BanchoMultiplayerBot.Web.Services;

public class LobbyService(
    ISnackbar snackbar,
    ApiService apiService)
{
    public List<LobbyState> Lobbies { get; } = [];

    public async Task<LobbyConfigurationModel> GetConfiguration(int id)
    {
        var response = await apiService.Get<LobbyConfigurationModel?>($"api/lobby/{id}/config");

        if (response == null)
        {
            snackbar.Add("Failed to get configuration for lobby", Severity.Error);
            return null!;
        }

        return response;
    }

    public async Task<bool> SaveConfiguration(int id, LobbyConfigurationModel model)
    {
        return await apiService.Post($"api/lobby/{id}/config", model);
    }

    public async Task<bool> RefreshLobby(int id, bool rejoinChannel = false)
    {
        return await apiService.Post($"api/lobby/{id}/refresh?rejoinChannel={rejoinChannel}");
    }
    
    public async Task GetLobbyExtended(int id)
    {
        var lobby = Lobbies.First(x => x.Id == id);
        var response = await apiService.Get<ReadLobbyExtended?>($"api/lobby/{id}");

        if (response == null)
        {
            snackbar.Add($"Unable to get lobby data for {id}", Severity.Error);
            return;
        }

        lobby.Name = response.Value.Name;
        lobby.Health = response.Value.Health;
        lobby.PlayerCount = response.Value.PlayerCount;
        lobby.PlayerCapacity = response.Value.PlayerCapacity;
        lobby.Players = response.Value.Players;
        lobby.Beatmap = response.Value.BeatmapInfo;
        lobby.Behaviors = response.Value.Behaviors;
        lobby.Host = response.Value.Host;
        lobby.HasExtended = true;
    }
    
    public async Task GetLobbies()
    {
        var lobbies = await apiService.Get<IReadOnlyList<ReadLobby>?>("api/lobby/list");

        if (lobbies == null)
        {
            snackbar.Add("Unable to get lobbies", Severity.Error);
            return;
        }

        Lobbies.Clear();

        foreach (var lobby in lobbies)
        {
            Lobbies.Add(new LobbyState(lobby.Id, lobby.Name, lobby.Health, lobby.PlayerCount, lobby.PlayerCapacity));
        }
    }

    public async Task<int?> CreateLobby(string name, string? channel)
    {
        var successful = await apiService.Post("api/lobby/create", new CreateLobby(name, channel));
        
        if (!successful)
        {
            snackbar.Add("Error creating lobby", Severity.Error);
            return null;
        }

        // We don't actually get the new lobby object back because the
        // backend sucks, so retrieve a list of the lobbies again.
        await GetLobbies();
        
        return Lobbies.OrderByDescending(x => x.Id).FirstOrDefault()?.Id;
    }
}