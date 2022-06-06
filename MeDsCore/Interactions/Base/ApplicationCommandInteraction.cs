using System.Text.Json;
using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest.Discord;
using MeDsCore.Rest.Extensions;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Methods;

namespace MeDsCore.Interactions.Base;

public class ApplicationCommandInteraction : InteractionBase<InteractionApplicationCommandResponse>
{
    private ApplicationCommandInteraction(InteractionBaseEntity entity, IMethodExecutor restApiExecutor, IDiscordGuild? sourceGuild) : base(entity, restApiExecutor, sourceGuild)
    {
        var appEntity = entity.Data.Deserialize<InteractionApplicationCommandResponseEntity>();

        if (appEntity == null) throw new NullReferenceException("Failed to deserialize Application Commands");

        Data = new InteractionApplicationCommandResponse(appEntity);
    }

    public static async Task<ApplicationCommandInteraction> InitializeAsync(InteractionBaseEntity entity,
        IMethodExecutor restApiExecutor)
    {
        if (entity.GuildId == null)
        {
            return new ApplicationCommandInteraction(entity, restApiExecutor, null);
        }

        var getGuildInfo = GuildMethods.ConfigureGetGuildMethodInfo(ulong.Parse(entity.GuildId));
        var guild = await restApiExecutor.ExecuteMethodAsync<GuildEntity>(getGuildInfo);

        return new ApplicationCommandInteraction(entity, restApiExecutor, new DiscordGuild(restApiExecutor, guild!));
    }
}