using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

public class UserCommandAttribute : ApplicationCommandAttribute
{
    public UserCommandAttribute(string name, bool dmPermission = false) :
        base(name, ApplicationCommandType.User, null, dmPermission)
    {
    }
}