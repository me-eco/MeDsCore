using System.Text.Json;
using MeDsCore.Interactions.Base;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest.Discord;
using MeDsCore.Rest.Net;

namespace MeDsCore.Interactions.ApplicationCommands;

public class ApplicationArgument
{
    public ApplicationArgument(InteractionResponseDataOption option, ResolvedData? resolvedData, IMethodExecutor methodExecutor)
    {
        Name = option.Name;
        Type = option.Type;
        var stringRep = option.Value.ToString();

        object DeserializeWithResolvedData()
        {
            var parsedId = ulong.Parse(stringRep);

            if (resolvedData == null)
                return option.Value.Deserialize<object>()!;
            
            return option.Type switch
            {
                ApplicationCommandOptionType.User => new DiscordUser(methodExecutor, resolvedData.Users![parsedId]),
                ApplicationCommandOptionType.Channel => DiscordChannel.CreateChannel(methodExecutor, resolvedData.Channels![parsedId]),
                ApplicationCommandOptionType.Role => new DiscordRole(resolvedData.Roles![parsedId]),
                _ => option.Value.Deserialize<object>()!
            };
        }
        
        Value = option.Type switch
        {
            ApplicationCommandOptionType.Boolean => option.Value.Deserialize<bool>(),
            ApplicationCommandOptionType.String => option.Value.Deserialize<string>()!,
            ApplicationCommandOptionType.Integer => option.Value.Deserialize<int>(),
            ApplicationCommandOptionType.Number => option.Value.Deserialize<double>(),
            _ => DeserializeWithResolvedData()
        };
    }

    public string Name { get; }
    public object Value { get; }
    public ApplicationCommandOptionType Type { get; }
}