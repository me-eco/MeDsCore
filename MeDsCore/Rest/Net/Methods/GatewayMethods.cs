namespace MeDsCore.Rest.Net.Methods;

public static class GatewayMethods
{
    public static DiscordMethodInfo ConfigureGetGatewayMethodInfo()
    {
        return DiscordMethodInfo.Get("/gateway");
    }
}