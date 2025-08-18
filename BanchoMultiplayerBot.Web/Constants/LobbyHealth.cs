namespace BanchoMultiplayerBot.Web.Constants;

public enum LobbyHealth
{
    Unknown = 0,
    ChannelCreationFailed,
    EventTimeoutReached,
    Stopped,
    Preparing,
    JoiningChannel,
    CreatingChannel,
    Initializing,
    Idle,
    Ok
}