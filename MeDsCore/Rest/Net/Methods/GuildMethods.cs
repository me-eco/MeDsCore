namespace MeDsCore.Rest.Net.Methods;

internal static class GuildMethods
{
    /// <summary>
    /// Метод получающий все каналы сервера
    /// </summary>
    /// <param name="guildId">Индетификатор сервера</param>
    public static DiscordMethodInfo ConfigureGetChannelsMethodInfo(ulong guildId) =>
        DiscordMethodInfo.Get($"/guilds/{guildId}/channels");
    
    /// <summary>
    /// Получает сервер по индефикатору
    /// </summary>
    /// <param name="guildId">Индефикатор сервера</param>
    /// <returns></returns>
    public static DiscordMethodInfo ConfigureGetGuildMethodInfo(ulong guildId) =>
        DiscordMethodInfo.Get($"/guilds/{guildId}");

    public static DiscordMethodInfo ConfigureListGuildMembers(ulong guildId) =>
        DiscordMethodInfo.Get($"/guilds/{guildId}/members");
    
    public static DiscordMethodInfo ConfigureGetMember(ulong guildId, ulong userId) => 
        DiscordMethodInfo.Get($"/guilds/{guildId}/members/{userId}");
}