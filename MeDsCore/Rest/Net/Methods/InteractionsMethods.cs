namespace MeDsCore.Rest.Net.Methods;

internal static class InteractionsMethods
{
    public static DiscordMethodInfo ConfigureResponseInteractionMethodInfo(ulong interactionId, string token)
    {
        return new DiscordMethodInfo($"/interactions/{interactionId}/{token}/callback", HttpMethod.Post);
    }
}