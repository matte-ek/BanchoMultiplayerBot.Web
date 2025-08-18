namespace BanchoMultiplayerBot.Web.State;

public class AuthenticationState
{
    public bool IsAuthenticated { get; set; }
    
    public string? OsuUsername { get; set; }

    public int? OsuId { get; set; }
}