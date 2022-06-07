using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Rest.Entities.Args;
using MeDsCore.Rest.Extensions;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Methods;

namespace MeDsCore.Rest.Discord;

public class DiscordUser : IDiscordUser
{
    private readonly IMethodExecutor _executor;

    internal DiscordUser(IMethodExecutor executor, UserEntity userEntity)
    {
        _executor = executor;
        Id = ulong.Parse(userEntity.Id);
        Username = userEntity.Username;
        IsBot = userEntity.Bot;
    }
    
    public ulong Id { get; }
    public string Username { get; }
    public bool IsBot { get; }
    
    public async Task<DiscordTextChannel> CreateDmChannelAsync()
    {
        var methodInfo = UserMethods.ConfigureCreateDmMessageMethodInfo();
        var entity = new CreateDmMessageArg()
        {
            RecipientId = Id.ToString()
        };

        var channelEntity = await _executor.ExecuteMethodAsync<ChannelEntity>(methodInfo, IContentBuilder.CreateJsonContentBuilder(entity));

        if (channelEntity == null)
        {
            throw new DiscordException("Failed to deserialize as ChannelEntity");
        }
        
        return new DiscordTextChannel(_executor, channelEntity);
    }
}