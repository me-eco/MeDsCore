namespace MeDsCore.Rest.Net.Methods;

public static class ChannelMethods
{
    /// <summary>
    /// Метод, который отправляет сообщение в канал
    /// </summary>
    /// <param name="channelId">Отправляет сообщение в канал</param>
    /// <returns></returns>
    public static DiscordMethodInfo ConfigureSendMessageAsyncMethodInfo(ulong channelId) =>
        DiscordMethodInfo.Post($"/channels/{channelId}/messages");

    public static DiscordMethodInfo ConfigureGetChannelMethodInfo(ulong channelId) =>
        DiscordMethodInfo.Get($"/channels/{channelId}");
}