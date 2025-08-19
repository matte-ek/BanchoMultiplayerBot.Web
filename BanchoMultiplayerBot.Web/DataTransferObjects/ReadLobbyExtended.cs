using BanchoMultiplayerBot.Web.Constants;
using BanchoMultiplayerBot.Web.Data;

namespace BanchoMultiplayerBot.Web.DataTransferObjects;

public record struct ReadLobbyExtended(
    int Id,
    string Name,
    LobbyHealth Health,
    int PlayerCount,
    int PlayerCapacity,
    BeatmapInfoModel Beatmap,
    List<string>? Behaviors,
    List<PlayerModel>? Players,
    PlayerModel? Host
    );