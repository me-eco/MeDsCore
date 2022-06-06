using System.Reflection;
using System.Text.Json.Serialization;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public class PrivateChatInputCommandInfo : PrivateApplicationCommandInfo
{
    [JsonPropertyName("options")]
    public OptionInfo[] Options { get; }
    [JsonPropertyName("description")]
    public string Description { get; }

    public PrivateChatInputCommandInfo(string name, string description, MethodInfo reflectionMethodInfo, ulong guildId, OptionInfo[] options, string? defaultMemberPermissions) : 
        base(name, ApplicationCommandType.ChatInput, reflectionMethodInfo, guildId, defaultMemberPermissions)
    {
        Options = options;
        Description = description;
    }
}