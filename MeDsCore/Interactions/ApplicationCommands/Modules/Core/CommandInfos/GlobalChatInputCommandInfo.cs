using System.Reflection;
using System.Text.Json.Serialization;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public class GlobalChatInputCommandInfo : GlobalApplicationCommandInfo
{
    [JsonPropertyName("options")]
    public OptionInfo[] Options { get; }
    [JsonPropertyName("description")]
    public string Description { get; }
    
    public GlobalChatInputCommandInfo(string name, string description, MethodInfo reflectionMethodInfo, bool dmPermission, OptionInfo[] options, string? defaultMemberPermissions) :
        base(name, ApplicationCommandType.ChatInput, reflectionMethodInfo, dmPermission, defaultMemberPermissions)
    {
        Options = options;
        Description = description;
    }
}