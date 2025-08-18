using BanchoMultiplayerBot.Web.Constants;

namespace BanchoMultiplayerBot.Web.DataTransferObjects;

public record struct ReadLobby(int Id, string Name, LobbyHealth Health, int PlayerCount, int PlayerCapacity);