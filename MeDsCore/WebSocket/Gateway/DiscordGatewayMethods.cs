using MeDsCore.WebSocket.Gateway.Entities;

namespace MeDsCore.WebSocket.Gateway;

public static class DiscordGatewayMethods
{
    public static DiscordGatewayMethodInfo CreateIdentifyMethodInfo(string token, int intents, string os, string browser, string device)
    {
        var data = new IdentifyEntity()
        {
            Token = token,
            Intents = intents,
            Properties = new IdentifyEntity.PropertiesEntity()
            {
                Os = os,
                Browser = browser,
                Device = device
            }
        };

        return new DiscordGatewayMethodInfo(GatewayClientOpcode.Identify, data);
    }

    public static DiscordGatewayMethodInfo CreateHeartbeatMethodInfo(int? d)
    {
#pragma warning disable CS8604
        return new DiscordGatewayMethodInfo(GatewayClientOpcode.Heartbeat, d);
#pragma warning restore CS8604
    }

    public static DiscordGatewayMethodInfo CreateResumeMethodInfo(string token, string sessionId, int seq) =>
        new DiscordGatewayMethodInfo(GatewayClientOpcode.Resume, new ResumeEntity()
        {
            Token = token,
            SessionId = sessionId,
            Seq = seq
        });
}