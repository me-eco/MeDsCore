using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ApplicationCommandAttribute : Attribute
{
    public ApplicationCommandAttribute(string name, ApplicationCommandType commandType, string? description, bool dmPermission)
    {
        Name = name;
        CommandType = commandType;
        Description = description;
        DmPermission = dmPermission;
    }

    public string Name { get; }
    public ApplicationCommandType CommandType { get; }
    public string? Description { get; }
    public bool DmPermission { get; }
}