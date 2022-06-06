namespace MeDsCore.WebSocket.Gateway;

public enum GatewayClientOpcode
{
    Heartbeat = 1,
    Identify = 2,
    PresenceUpdate = 3,
    VoiceStateUpdate = 4,
    Resume = 5,
    RequestGuildMembers = 8
}