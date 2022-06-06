using System.Net.Http;
using MeDsCore.Rest.Net;
using Xunit;

namespace MeDsCoreTests.Rest;

public class DiscordMethodConfigTests
{
    [Fact]
    public void DiscordMethodConfiguration()
    {
        var discordMethodConfig = new DiscordMethodInfo("/test", HttpMethod.Get);

        HttpRequestMessage message = discordMethodConfig.ConfigureScaffoldHttpRequest();

        Assert.Equal(HttpMethod.Get, message.Method);
        Assert.NotNull(message.RequestUri);
        Assert.Equal("/test", message.RequestUri!.ToString());
    }
}