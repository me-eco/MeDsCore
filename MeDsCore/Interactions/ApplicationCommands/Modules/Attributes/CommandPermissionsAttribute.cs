using MeDsCore.Utils;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]

public class CommandPermissionsAttribute : Attribute
{
    public long Permissions { get; }

    public CommandPermissionsAttribute(Permission permission)
    {
        Permissions = (long)permission;
    }
}