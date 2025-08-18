using BanchoMultiplayerBot.Web.Constants;
using BanchoMultiplayerBot.Web.Data;

namespace BanchoMultiplayerBot.Web.DataTransferObjects;

public record struct ReadLobbyExtended(
    int Id,
    string Name,
    LobbyHealth Health,
    int PlayerCount,
    int PlayerCapacity,
    BeatmapInfoModel BeatmapInfo,
    IEnumerable<string>? Behaviors,
    IEnumerable<PlayerModel>? Players,
    PlayerModel? Host
    );