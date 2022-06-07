using System.IO;
using System.Threading.Tasks;
using MeDsCore.Base;
using MeDsCore.Rest.Discord;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using Moq;
using Xunit;

namespace MeDsCoreTests.Rest.Implementations.Channels;

public class SendMessageTests
{
    [Fact]
    public async Task SendMessage_Should_Send()
    {
        var methodExecutorMock = new Mock<IMethodExecutor>();
        
        methodExecutorMock
            .Setup(x => x.ExecuteMethodAsync(It.IsAny<DiscordMethodInfo>(), It.IsAny<IContentBuilder>()))
            .ReturnsAsync(new DiscordMethodResult(true, new MemoryStream(), null));

        var channel = DiscordChannel.CreateChannel(methodExecutorMock.Object, new ChannelEntity()
        {
            Id = "0",
            Name = "1",
            Type = ChannelType.GuildText
        });


        Assert.IsType<DiscordTextChannel>(channel);
        
        await ((DiscordTextChannel)channel).SendMessageAsync("1");
    }

    [Fact]
    public void SendMessage_Shouldnt_Send_Because_Not_Txt_Channel()
    {
        var methodExecutorMock = new Mock<IMethodExecutor>();
        
        methodExecutorMock
            .Setup(x => x.ExecuteMethodAsync(It.IsAny<DiscordMethodInfo>(), It.IsAny<IContentBuilder>()))
            .ReturnsAsync(new DiscordMethodResult(true, new MemoryStream(), null));

        var channel = DiscordChannel.CreateChannel(methodExecutorMock.Object, new ChannelEntity()
        {
            Id = "0",
            Name = "1",
            Type = ChannelType.GuildVoice
        });

        Assert.IsNotType<DiscordTextChannel>(channel);
    }
}