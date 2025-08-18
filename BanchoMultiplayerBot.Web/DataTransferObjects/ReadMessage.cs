namespace BanchoMultiplayerBot.Web.DataTransferObjects;

public record struct ReadMessage(int Id, string Author, string Content, DateTime Timestamp, bool IsAdministratorMessage, bool IsBanchoMessage);