using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest.Net.Content;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public abstract class ApplicationCommandInfo
{
    public ApplicationCommandInfo(string name, ApplicationCommandType type, MethodInfo reflectionMethodInfo, string? defaultMemberPermissions)
    {
        Name = name;
        Type = type;
        ReflectionMethodInfo = reflectionMethodInfo;
        DefaultMemberPermissions = defaultMemberPermissions;
    }
    
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("type")]
    public ApplicationCommandType Type { get; }

    [JsonPropertyName("default_member_permissions")]
    public string? DefaultMemberPermissions { get; }

    [JsonIgnore]
    public MethodInfo ReflectionMethodInfo { get; }

    internal IContentBuilder ProvideContentBuilder()
    {
        Console.WriteLine(JsonSerializer.Serialize(this, GetType()));
        return new JsonContentBuilder(this);
    }
}