using System.Reflection;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public class GlobalMessageCommandInfo : GlobalApplicationCommandInfo
{
    public GlobalMessageCommandInfo(string name, MethodInfo reflectionMethodInfo, bool dmPermission, string? defaultMemberPermissions) : 
        base(name, ApplicationCommandType.Message, reflectionMethodInfo, dmPermission, defaultMemberPermissions)
    {
    }
}