using MeDsCore.Rest.Net;

namespace MeDsCore.Interactions.ApplicationCommands;

internal static class ApplicationCommandsMethods
{
    public static DiscordMethodInfo ConfigureCreateGlobalApplicationCommand(ulong applicationId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/commands", HttpMethod.Post);
    }

    public static DiscordMethodInfo ConfigureCreatePrivateApplicationCommand(ulong applicationId, ulong guildId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/guilds/{guildId}/commands", HttpMethod.Post);
    }

    public static DiscordMethodInfo ConfigureEditGlobalApplicationCommand(ulong applicationId, ulong commandId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/commands/{commandId}", HttpMethod.Patch);
    }

    public static DiscordMethodInfo ConfigureEditPrivateApplicationCommand(ulong applicationId, ulong guildId, ulong commandId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/guilds/{guildId}/commands/{commandId}",
            HttpMethod.Patch);
    }

    public static DiscordMethodInfo ConfigureGetGlobalApplicationCommands(ulong applicationId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/commands", HttpMethod.Get);
    }

    public static DiscordMethodInfo ConfigureGetPrivateApplicationCommands(ulong applicationId, ulong guildId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/guilds/{guildId}", HttpMethod.Get);
    }

    public static DiscordMethodInfo ConfigureDeletePrivateApplicationCommand(ulong applicationId, ulong guildId, ulong commandId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/guilds/{guildId}/commands/{commandId}",
            HttpMethod.Delete);
    }
    
    public static DiscordMethodInfo ConfigureDeleteGlobalApplicationCommand(ulong applicationId, ulong commandId)
    {
        return new DiscordMethodInfo($"/applications/{applicationId}/commands/{commandId}", HttpMethod.Delete);
    }
}