using System.Reflection;
using System.Text.Json.Serialization;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public abstract class GlobalApplicationCommandInfo : ApplicationCommandInfo
{
    [JsonPropertyName("dm_permission")]
    public bool DmPermission { get; }

    public GlobalApplicationCommandInfo(string name, ApplicationCommandType type, MethodInfo reflectionMethodInfo, bool dmPermission, string? defaultMemberPermissions) :
        base(name, type, reflectionMethodInfo, defaultMemberPermissions)
    {
        DmPermission = dmPermission;
    }
}