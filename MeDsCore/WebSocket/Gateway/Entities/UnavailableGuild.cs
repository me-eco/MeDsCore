using MeDsCore.Base;

namespace MeDsCore.WebSocket.Gateway.Entities;

public class UnavailableGuild
{
    public UnavailableGuild(GuildEntity guildEntity)
    {
        Id = ulong.Parse(guildEntity.Id);
    }
    
    public ulong Id { get; }
}