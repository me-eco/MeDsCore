using MeDsCore.Base;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.Base;

/// <summary>
/// Предстваляет собой репрезентацию комнады приложения
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

public class ApplicationCommandOption
{
    public ApplicationCommandOption(ApplicationCommandOptionEntity entity)
    {
        Type = entity.Type;
        Name = entity.Name;
        Description = entity.Description;
        Required = entity.Required;
        
        #region CHOICES_IMPL

        var choices = new ApplicationCommandOptionChoice[entity.Choices.Length];
        
        for (var i = 0; i < choices.Length; i++)
        {
            choices[i] = new ApplicationCommandOptionChoice(entity.Choices[i]);
        }

        Choices = choices;

        #endregion
        
        #region OPTIONS_IMPL

        var options = new ApplicationCommandOption[entity.Options.Length];

        for (var i = 0; i < options.Length; i++)
        {
            options[i] = new ApplicationCommandOption(entity.Options[i]);
        }

        Options = options;

        #endregion

        ChannelTypes = entity.ChannelTypes;
        MinValue = entity.MinValue;
        MaxValue = entity.MaxValue;
        Autocomplete = entity.Autocomplete;
    }
    
    public ApplicationCommandOptionType Type { get; }
    public string Name { get; }
    public string Description { get; }
    public bool Required { get; }
    public ApplicationCommandOptionChoice[] Choices { get; }
    public ApplicationCommandOption[] Options { get; }
    public ChannelType[] ChannelTypes { get; }
    public object MinValue { get; }
    public object MaxValue { get; }
    public bool Autocomplete { get; }
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