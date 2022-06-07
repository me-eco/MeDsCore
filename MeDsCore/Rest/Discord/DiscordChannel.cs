using System.Text.Json.Serialization;
using MeDsCore.Rest.Entities;
using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Rest.Net;

namespace MeDsCore.Rest.Discord;

public class DiscordChannel : IDiscordChannel
{
    internal IMethodExecutor MethodExecutor { get; }

    internal DiscordChannel(IMethodExecutor methodExecutor, ChannelEntity channelEntity)
    {
        MethodExecutor = methodExecutor;
        Id = ulong.Parse(channelEntity.Id);
        Name = channelEntity.Name;
        ChannelType = channelEntity.Type;
    }
    
    public ulong Id { get; }
    public string Name { get; }
    public ChannelType ChannelType { get; }

    internal static DiscordChannel CreateChannel(IMethodExecutor executor, ChannelEntity entity)
    {
        if (entity.Type == ChannelType.GuildText) return new DiscordTextChannel(executor, entity);
        return new DiscordChannel(executor, entity);
    }
}