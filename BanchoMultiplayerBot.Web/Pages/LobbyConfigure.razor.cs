using BanchoMultiplayerBot.Web.Data;
using BanchoMultiplayerBot.Web.Services;
using BanchoMultiplayerBot.Web.State;
using Microsoft.AspNetCore.Components;

namespace BanchoMultiplayerBot.Web.Pages;

public partial class LobbyConfigure(LobbyService lobbyService) : ComponentBase
{
    [Parameter]
    public int LobbyId { get; set; }
    
    private LobbyState State { get; set; } = null!;
    
    private LobbyConfigurationModel ConfigurationModel { get; set; } = new();

    private IEnumerable<string> _tempSelectedBehaviors = null!;
    
    private async Task OnBehaviorSelectionClosed()
    {
        var config = await lobbyService.GetConfiguration(LobbyId);
        
        config.Behaviours = _tempSelectedBehaviors.ToArray();
        
        Console.WriteLine(string.Join(", ", config.Behaviours));
        
        if (await lobbyService.SaveConfiguration(LobbyId, config))
        {
            ConfigurationModel.Behaviours = _tempSelectedBehaviors.ToArray();
        }
    }
    
    protected override async Task OnParametersSetAsync()
    {
        State = lobbyService.Lobbies.First(x => x.Id == LobbyId);
        ConfigurationModel = await lobbyService.GetConfiguration(LobbyId);
        _tempSelectedBehaviors = ConfigurationModel.Behaviours ?? [];
        
        await base.OnParametersSetAsync();
    }
}