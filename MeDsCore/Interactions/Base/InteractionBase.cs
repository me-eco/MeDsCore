using System.Text.Json;
using MeDsCore.Abstractions;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest.Discord;
using MeDsCore.Rest.Net;

namespace MeDsCore.Interactions.Base;

public class InteractionBase<TData>
{
    public InteractionBase(InteractionBaseEntity entity, IMethodExecutor restApiExecutor, IDiscordGuild? sourceGuild)
    {
        ulong ParseUlong(string snowflake)
        {
            return ulong.Parse(snowflake);
        }

        Id = ParseUlong(entity.Id);
        ApplicationId = ParseUlong(entity.ApplicationId);
        Type = entity.Type;

        GuildId = ParseUlong(entity.GuildId);
        Member = sourceGuild != null ? new DiscordGuildMember(restApiExecutor, entity.Member, sourceGuild) : null;
        //User = new DiscordUser(restApiExecutor, entity.User);
        ChannelId = ParseUlong(entity.ChannelId);
        Token = entity.Token;
        //Message = new DiscordMessage(restApiExecutor, entity.Message);
        Locale = entity.Locale;
        GuildLocale = entity.GuildLocale;
    }

    public ulong Id { get; }
    public ulong ApplicationId { get; }
    public InteractionType Type { get; }
    public TData Data { get; protected set; }
    public ulong GuildId { get; }
    public ulong ChannelId { get; }
    public DiscordGuildMember? Member { get; }
    public DiscordUser User { get; }
    public string Token { get; }
    public DiscordMessage Message { get; }
    public string Locale { get; }
    public string GuildLocale { get; }
}