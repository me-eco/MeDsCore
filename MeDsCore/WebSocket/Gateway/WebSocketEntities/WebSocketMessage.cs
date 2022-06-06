using MeDsCore.Base;
using MeDsCore.Rest.Discord;
using MeDsCore.Rest.Net;

namespace MeDsCore.WebSocket.Gateway.WebSocketEntities;

public sealed class WebSocketMessage : DiscordMessage
{
    private WebSocketMessage(IMethodExecutor executor, MessageEntity entity) : base(executor, entity)
    {
        
    }
    
    public DiscordUser Author { get; private set; } = null!;
    public DiscordGuildMember? Member { get; private set; }

    public new static async Task<WebSocketMessage> InitializeAsync(IMethodExecutor executor, MessageEntity entity)
    {
        var wsMessage = new WebSocketMessage(executor, entity);
        await wsMessage.LoadAsync();

        if (wsMessage.Guild != null)
        {
            wsMessage.Member = new DiscordGuildMember(executor, entity.Member!, entity.Author!, wsMessage.Guild);
            wsMessage.Author = wsMessage.Member;
        }
        else
        {
            wsMessage.Author = new DiscordUser(executor, entity.Author!);
        }

        return wsMessage;
    }
}