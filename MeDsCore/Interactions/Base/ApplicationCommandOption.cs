using MeDsCore.Base;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.Base;

/// <summary>
/// Application command option model
/// </summary>
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