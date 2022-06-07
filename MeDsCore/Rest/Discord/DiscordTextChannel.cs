using MeDsCore.Base;
using MeDsCore.Rest.Entities.Args;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Methods;

namespace MeDsCore.Rest.Discord;

public class DiscordTextChannel : DiscordChannel
{
    internal DiscordTextChannel(IMethodExecutor executor, ChannelEntity entity) : base(executor, entity)
    {
        
    }
    
    public async Task SendMessageAsync(string content)
    {
        var methodInfo = ChannelMethods.ConfigureSendMessageAsyncMethodInfo(Id);

        var apiArg = new SendMessageArg
        {
            Content = content
        };

        var contentBuilder = IContentBuilder.CreateJsonContentBuilder(apiArg);

        var result = await MethodExecutor.ExecuteMethodAsync(methodInfo, contentBuilder);

        if (!result.IsSuccess)
        {
            throw new DiscordDetailizedException(result.Error!);
        }
    }
}