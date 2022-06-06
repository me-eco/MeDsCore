using MeDsCore.Rest.Extensions;
using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Rest.Discord;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Methods;
using MeDsCore.Rest.Net.Proxy;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Rest;

public class DiscordRestClient
{
    internal DiscordMethodExecutor MethodExecutor { get; }
    
    public DiscordRestClient(string baseClientUrl, AuthorizationProvider authorizationProvider, ILogger logger)
    {
        MethodExecutor = new DiscordMethodExecutor(new DiscordProxy(baseClientUrl, logger),
            authorizationProvider, logger);
    }

    public async Task<IDiscordGuild?> GetGuildAsync(ulong guildId)
    {
        var methodInfo = GuildMethods.ConfigureGetGuildMethodInfo(guildId);
        var guildEntity = await MethodExecutor.ExecuteMethodAsync<GuildEntity>(methodInfo);

        return guildEntity == null ? null : new DiscordGuild(MethodExecutor, guildEntity);
    }
}
