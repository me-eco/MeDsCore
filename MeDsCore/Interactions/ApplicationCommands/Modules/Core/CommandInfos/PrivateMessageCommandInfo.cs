using System.Reflection;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public class PrivateMessageCommandInfo : PrivateApplicationCommandInfo
{
    public PrivateMessageCommandInfo(string name, MethodInfo reflectionMethodInfo, ulong guildId, string? defaultMemberPermissions) :
        base(name, ApplicationCommandType.Message, reflectionMethodInfo, guildId, defaultMemberPermissions)
    {
    }
}