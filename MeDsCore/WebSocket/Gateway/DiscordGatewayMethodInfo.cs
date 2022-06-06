namespace MeDsCore.WebSocket.Gateway;

/// <summary>
/// Base for all gateway method
/// </summary>
public class DiscordGatewayMethodInfo
{
    public DiscordGatewayMethodInfo(GatewayClientOpcode opCode, object jsonPayload)
    {
        OpCode = opCode;
        JsonPayload = jsonPayload;
    }

    public GatewayClientOpcode OpCode { get; }
    public object JsonPayload { get; }
}