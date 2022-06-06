using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.Base;

/// <summary>
/// Application command model
/// </summary>
public class ApplicationCommand
{
    public ApplicationCommand(ApplicationCommandEntity entity)
    {
        Id = ulong.Parse(entity.Id);
        ApplicationId = ulong.Parse(entity.ApplicationId);
        GuildId = ulong.Parse(entity.GuildId);
        Type = entity.Type;
        Name = entity.Name;
        Description = entity.Description;
        #region OPTIONS_IMPL

        var options = new ApplicationCommandOption[entity.Options.Length];
        
        for (var i = 0; i < options.Length; i++)
        {
            options[i] = new ApplicationCommandOption(entity.Options[i]);
        }

        Options = options;

        #endregion
        DmPermission = entity.DmPermission;
        DefaultPermission = entity.DefaultPermission;
        Version = ulong.Parse(entity.Version);
    }
    
    public ulong Id { get; }
    public ulong ApplicationId { get; }
    public ulong GuildId { get; }
    public ApplicationCommandType Type { get; }
    public string Name { get; }
    public string Description { get; }
    public ApplicationCommandOption[] Options { get; }
    public bool DmPermission { get; }
    public bool DefaultPermission { get; }
    public ulong Version { get; }
}

public class ApplicationCommandOptionChoice
{
    public ApplicationCommandOptionChoice(ApplicationCommandOptionChoiceEntity entity)
    {
        Name = entity.Name;
        Value = entity.Value;
    }
    
    public string Name { get; }
    public object Value { get; }
}