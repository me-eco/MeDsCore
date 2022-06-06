using System.Text.Json;
using MeDsCore.Base;
using Xunit;

namespace MeDsCoreTests.Rest;

public class DiscordChannelDeserialingTest
{
    [Fact]
    public void Must_Deserialize_Discord_Guild_Txt()
    {
        const string json = "{\"id\": \"41771983423143937\",\"name\": \"general\",\"type\": 1}";
        var guildEntity = JsonSerializer.Deserialize<ChannelEntity>(json);
        
        Assert.NotNull(guildEntity);
        Assert.Equal(ChannelType.Dm, guildEntity!.Type);
    }
}