namespace BanchoMultiplayerBot.Web.State;

public class HealthState
{
    public bool HasConfigurationError  { get; set; }
    
    public bool IsBanchoConnected { get; set; }
    
    public bool IsLobbyActive { get; set; }
}