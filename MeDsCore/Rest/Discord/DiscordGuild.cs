using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Rest.Extensions;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Methods;

namespace MeDsCore.Rest.Discord;

public class DiscordGuild : IDiscordGuild
{
    private readonly IMethodExecutor _methodExecutor;

    public DiscordGuild(IMethodExecutor methodExecutor, GuildEntity entity)
    {
        _methodExecutor = methodExecutor;
        Id = ulong.Parse(entity.Id);
        Name = entity.Name;
        GuildMembers = GetGuildMembersAsync();
        Channels = GetChannelsAsync();
        TextChannels = GetTextChannelsAsync();
    }

    public ulong Id { get; }
    public string Name { get; }
    public IAsyncEnumerable<DiscordGuildMember> GuildMembers { get; }
    public IAsyncEnumerable<IDiscordChannel> Channels { get; }
    public IAsyncEnumerable<DiscordTextChannel> TextChannels { get; }

    public async IAsyncEnumerable<IDiscordChannel> GetChannelsAsync()
    {
        var channelEntities = await FetchChannelsAsync();
        
        foreach (var channelEntity in channelEntities)
        {
            yield return DiscordChannel.CreateChannel(_methodExecutor, channelEntity);
        }
    }

    public async IAsyncEnumerable<DiscordTextChannel> GetTextChannelsAsync()
    {
        var channelEntities = await FetchChannelsAsync();

        foreach (var channelEntity in channelEntities)
        {
            if (channelEntity.Type == ChannelType.GuildText)
            {
                yield return new DiscordTextChannel(_methodExecutor, channelEntity); 
            }
        }
    }

    public async Task<DiscordGuildMember?> GetMemberAsync(ulong id)
    {
        var getMemConfig = GuildMethods.ConfigureGetMember(Id, id);
        var guildMemberEntity = await _methodExecutor.ExecuteMethodAsync<GuildMemberEntity>(getMemConfig);

        if (guildMemberEntity == null) return null;
        
        return new DiscordGuildMember(_methodExecutor, guildMemberEntity, this);
    }

    public async IAsyncEnumerable<DiscordGuildMember> GetGuildMembersAsync()
    {
        var getMemsMethodInfo = GuildMethods.ConfigureListGuildMembers(Id);
        var memberEntities = await _methodExecutor.ExecuteMethodAsync<GuildMemberEntity[]>(getMemsMethodInfo);
        
        if(memberEntities == null) yield break;
        
        foreach (var memberEntity in memberEntities)
        {
            yield return new DiscordGuildMember(_methodExecutor, memberEntity, this);
        }
    }

    private Task<ChannelEntity[]> FetchChannelsAsync()
    {
        var methodInfo = GuildMethods.ConfigureGetChannelsMethodInfo(Id);
        
        return _methodExecutor.ExecuteMethodAsync<ChannelEntity[]>(methodInfo)!;
    }
}