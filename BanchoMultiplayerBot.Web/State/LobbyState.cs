using BanchoMultiplayerBot.Web.Constants;
using BanchoMultiplayerBot.Web.Data;

namespace BanchoMultiplayerBot.Web.State;

public class LobbyState(int id, string name, LobbyHealth health, int playerCount, int playerCapacity)
{
    public bool HasExtended { get; set; }
    
    public bool IsInMatch { get; set; }

    public int Id { get; set; } = id;

    public string Name { get; set; } = name;

    public LobbyHealth Health { get; set; } = health;

    public int PlayerCount { get; set; } = playerCount;

    public int PlayerCapacity { get; set; }  = playerCapacity;

    public BeatmapInfoModel? Beatmap { get; set; }

    public IEnumerable<string>? Behaviors { get; set; }

    public List<PlayerModel>? Players { get; set; }

    public PlayerModel? Host { get; set; }
}