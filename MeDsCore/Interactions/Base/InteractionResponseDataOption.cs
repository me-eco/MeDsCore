using System.Text.Json;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.Base;

internal class InteractionResponseDataOption
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
}
