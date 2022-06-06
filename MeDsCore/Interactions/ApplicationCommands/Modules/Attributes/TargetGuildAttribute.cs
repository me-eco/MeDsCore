namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class TargetGuildAttribute : Attribute
{
    public TargetGuildAttribute(ulong id)
    {
        Id = id;
    }

    public ulong Id { get; }
}