namespace MeDsCore.WebSocket.Gateway;

public enum GatewayServerOpcode
{
    Dispatch = 0,
    Heartbeat = 1,
    Reconnect = 7,
    InvalidSession = 9,
    Hello = 10,
    HeartbeatAck = 11
}