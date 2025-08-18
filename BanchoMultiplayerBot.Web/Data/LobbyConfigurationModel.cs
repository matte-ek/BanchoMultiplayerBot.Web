namespace BanchoMultiplayerBot.Web.Data;

public class LobbyConfigurationModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = "Unnamed";

    public int Mode { get; set; } = 0;

    public int TeamMode { get; set; } = 0;

    public int ScoreMode { get; set; } = 0;

    public string[] Mods { get; set; } = [];

    public int Size { get; set; } = 16;

    public string Password { get; set; } = string.Empty;

    public string[]? Behaviours { get; set; }
}