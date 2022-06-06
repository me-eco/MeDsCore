using System.Text.Json;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.Base;

public class InteractionResponseDataOption
{
    public InteractionResponseDataOption(InteractionResponseDataOptionEntity entity)
    {
        Name = entity.Name;
        Type = entity.Type;
        Value = entity.Value;
    }
    
    public string Name { get; }
    public ApplicationCommandOptionType Type { get; }
    public JsonElement Value { get; }

    public InteractionResponseDataOption Create(InteractionResponseDataOptionEntity entity)
    {
        return entity.Type switch
        {
            ApplicationCommandOptionType.String => new InteractionResponseStringDataOption(entity),
            _ => new InteractionResponseDataOption(entity)
        };
    }
}

public class InteractionResponseStringDataOption : InteractionResponseDataOption
{
    public InteractionResponseStringDataOption(InteractionResponseDataOptionEntity entity) : base(entity)
    {
        if (entity.Type != ApplicationCommandOptionType.String)
            throw new InvalidCastException("Failed to convert JsonElement option to String");
        OptionValue = entity.Value.Deserialize<string>()!;
    }
    
    public string OptionValue { get; set; }
}