namespace MeDsCore.Rest.Net.Methods;

internal static class MessageMethods
{
    public static DiscordMethodInfo ConfigureDeleteMessageMethodInfo(ulong channelId, ulong messageId) =>
        DiscordMethodInfo.Delete($"/channels/{channelId}/messages/{messageId}");
}