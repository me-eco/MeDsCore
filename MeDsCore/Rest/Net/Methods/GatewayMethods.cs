namespace MeDsCore.Rest.Net.Methods;

internal static class GatewayMethods
{
    public static DiscordMethodInfo ConfigureGetGatewayMethodInfo()
    {
        return DiscordMethodInfo.Get("/gateway");
    }
}