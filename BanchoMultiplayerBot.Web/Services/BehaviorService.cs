namespace BanchoMultiplayerBot.Web.Services;

public class BehaviorService(ApiService apiService)
{
    public async Task<T> GetConfiguration<T>(int lobbyId, string name) =>
        (await apiService.Get<T>($"api/behavior/{lobbyId}/{name}"))!;
    
    public async Task<bool> SaveConfiguration<T>(int lobbyId, string name, T configuration) => 
        await apiService.Post($"api/behavior/{lobbyId}/{name}", configuration);
}