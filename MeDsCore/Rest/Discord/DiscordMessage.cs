using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Rest.Extensions;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Methods;

namespace MeDsCore.Rest.Discord;

public class DiscordMessage : IDiscordMessage
{
    private readonly IMethodExecutor _executor;

    protected DiscordMessage(IMethodExecutor executor, MessageEntity entity)
    {
        _executor = executor;
        Id = ulong.Parse(entity.Id);
        Content = entity.Content;
        ChannelId = ulong.Parse(entity.ChannelId);
        GuildId = entity.GuildId != null ? ulong.Parse(entity.GuildId) : null;
    }
    
    public ulong Id { get; }
    public string Content { get; }
    public ulong ChannelId { get; }
    public DiscordTextChannel Channel { get; private set; }
    public ulong? GuildId { get; }
    public DiscordGuild? Guild { get; private set; }
    
    private async Task<DiscordTextChannel> GetSourceChannelAsync()
    {
        var getChannelInfo = ChannelMethods.ConfigureGetChannelMethodInfo(ChannelId);
        var channelEntity = await _executor.ExecuteMethodAsync<ChannelEntity>(getChannelInfo, IContentBuilder.EmptyContent);

        return new DiscordTextChannel(_executor, channelEntity!);
    }

    private async Task<DiscordGuild?> GetSourceGuildAsync()
    {
        if (GuildId == null) return null;
        var getGuildInfo = GuildMethods.ConfigureGetGuildMethodInfo(GuildId.Value);

        var guildEntity = (await _executor.ExecuteMethodAsync<GuildEntity>(getGuildInfo))!;

        return new DiscordGuild(_executor, guildEntity);
    }
    
    public async Task DeleteAsync()
    {
        var messageDeleteInfo = MessageMethods.ConfigureDeleteMessageMethodInfo(ChannelId, Id);
        var result = await _executor.ExecuteMethodAsync(messageDeleteInfo, IContentBuilder.EmptyContent);
        
        if (!result.IsSuccess) throw new DiscordDetailizedException(result.Error!);
    }

    protected async Task LoadAsync()
    {
        Channel = await GetSourceChannelAsync();
        Guild = await GetSourceGuildAsync();
    }
    
    public static async Task<DiscordMessage> InitializeAsync(IMethodExecutor executor, MessageEntity entity)
    {
        var msg = new DiscordMessage(executor, entity);

        msg.Channel = await msg.GetSourceChannelAsync();
        msg.Guild = await msg.GetSourceGuildAsync();

        return msg;
    }
}