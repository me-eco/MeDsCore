using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

public class MessageCommandAttribute : ApplicationCommandAttribute
{
    public MessageCommandAttribute(string name, bool dmPermission = false) :
        base(name, ApplicationCommandType.Message, null, dmPermission)
    {
    }
}