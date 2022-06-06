namespace MeDsCore.WebSocket.Gateway;

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