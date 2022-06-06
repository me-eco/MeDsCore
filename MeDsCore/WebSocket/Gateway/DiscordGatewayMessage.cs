using System.Text.Json;

namespace MeDsCore.WebSocket.Gateway;

public class DiscordGatewayMessage
{
    public DiscordGatewayMessage(GatewayServerOpcode opcode, JsonElement? content, int? sequence, string? eventName)
    {
        Opcode = opcode;
        Content = content;
        Sequence = sequence;
        EventName = eventName;
    }

    public GatewayServerOpcode Opcode { get; }
    public JsonElement? Content { get; }
    public int? Sequence { get; }
    public string? EventName { get; }

    public T ConvertJson<T>() where T : class
    {
        return Content!.Value.Deserialize<T>()!;
    }
}