namespace BanchoMultiplayerBot.Web.DataTransferObjects;

public record struct WriteMessage(int LobbyId, string Content);