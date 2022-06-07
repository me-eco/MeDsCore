namespace MeDsCore.Rest.Net.Methods;

internal static class UserMethods
{
    public static DiscordMethodInfo ConfigureCreateDmMessageMethodInfo()
    {
        return DiscordMethodInfo.Post($"/users/@me/channels");
    }

    public static DiscordMethodInfo ConfigureAddGuildMemberRole(ulong guildId, ulong userId, ulong roleId)
    {
        return DiscordMethodInfo.Put($"/guilds/{guildId}/members/{userId}/roles/{roleId}");
    }
}