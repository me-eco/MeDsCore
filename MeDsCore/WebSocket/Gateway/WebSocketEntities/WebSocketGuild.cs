using MeDsCore.Base;
using MeDsCore.Rest.Discord;
using MeDsCore.Rest.Net;

namespace MeDsCore.WebSocket.Gateway.WebSocketEntities;

/// <summary>
/// Guild from WS
/// </summary>
public class WebSocketGuild : DiscordGuild
{
    internal WebSocketGuild(IMethodExecutor executor, GuildEntity guildEntity) : base(executor, guildEntity)
    {
        if (guildEntity.Channels == null || guildEntity.Members == null)
        {
            throw new NullReferenceException("Failed to create WS Guild instance");
        }

        Channels = guildEntity.Channels.Select(x => DiscordChannel.CreateChannel(executor, x));
        Members = guildEntity.Members.Select(x => new DiscordGuildMember(executor, x, this));
    }
    
    /// <summary>
    /// Guild's channels
    /// </summary>
    public IEnumerable<DiscordChannel> Channels { get; }
    /// <summary>
    /// Guild's members
    /// </summary>
    public IEnumerable<DiscordGuildMember> Members { get; }
}
