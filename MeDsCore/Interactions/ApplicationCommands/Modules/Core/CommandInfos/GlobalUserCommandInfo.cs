using System.Reflection;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public class GlobalUserCommandInfo : GlobalApplicationCommandInfo
{
    public GlobalUserCommandInfo(string name, MethodInfo reflectionMethodInfo, bool dmPermission, string? defaultMemberPermissions) :
        base(name, ApplicationCommandType.User, reflectionMethodInfo, dmPermission, defaultMemberPermissions)
    {
    }
}