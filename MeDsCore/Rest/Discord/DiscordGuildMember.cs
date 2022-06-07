using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Methods;

namespace MeDsCore.Rest.Discord;

public class DiscordGuildMember : DiscordUser
{
    private readonly IMethodExecutor _executor;

    internal DiscordGuildMember(IMethodExecutor executor, GuildMemberEntity memberEntity, IDiscordGuild source) : base(executor,
        memberEntity.User!)
    {
        _executor = executor;
        Guild = source;
        Nickname = memberEntity.Nick!;
    }

    internal DiscordGuildMember(IMethodExecutor executor, GuildMemberEntity memberEntity, UserEntity userEntity, IDiscordGuild source) : base(executor,
        userEntity)
    {
        _executor = executor;
        Guild = source;
        Nickname = memberEntity.Nick!;
    }
    
    public string Nickname { get; }
    public IDiscordGuild Guild { get; }

    public async Task AddRoleAsync(ulong roleId)
    {
        var addRoleMethodInfo = UserMethods.ConfigureAddGuildMemberRole(Guild.Id, Id, roleId);
        await _executor.ExecuteMethodAsync(addRoleMethodInfo, IContentBuilder.EmptyContent);
    }
}