using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

public class SlashCommandAttribute : ApplicationCommandAttribute
{
    public SlashCommandAttribute(string name, string description, bool dmPermission = false) :
        base(name, ApplicationCommandType.ChatInput, description, dmPermission)
    {
    }
}