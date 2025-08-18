namespace BanchoMultiplayerBot.Web.DataTransferObjects;

public record struct CreateLobby(string Name, string? PreviousChannel);