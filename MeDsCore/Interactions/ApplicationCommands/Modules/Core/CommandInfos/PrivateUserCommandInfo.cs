using System.Reflection;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public class PrivateUserCommandInfo : PrivateApplicationCommandInfo
{
    public PrivateUserCommandInfo(string name, MethodInfo reflectionMethodInfo, ulong guildId, string? defaultMemberPermissions) :
        base(name, ApplicationCommandType.User, reflectionMethodInfo, guildId, defaultMemberPermissions)
    {
    }
}