using System.Text.Json.Serialization;
using MeDsCore.Base;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules;

/// <summary>
/// Contains information of an option of an applicaiton command
/// </summary>
public class OptionInfo
{
    public OptionInfo(ApplicationCommandOptionType type, string name, string description, 
        OptionChoice[]? choices, ChannelType[]? channelsTypes, object? minValue, object? maxValue, bool isRequired)
    {
        Type = type;
        Name = name;
        Description = description;
        Choices = choices;
        ChannelsTypes = channelsTypes;
        MinValue = minValue;
        MaxValue = maxValue;
        IsRequired = isRequired;
    }
    
    [JsonPropertyName("type")]
    public ApplicationCommandOptionType Type { get; }
    [JsonPropertyName("name")]
    public string Name { get; }
    [JsonPropertyName("description")]
    public string Description { get; }
    [JsonPropertyName("choices")]
    public OptionChoice[]? Choices { get; }
    [JsonPropertyName("channels_types")]
    public ChannelType[]? ChannelsTypes { get; }
    [JsonPropertyName("min_value")]
    public object? MinValue { get; }
    [JsonPropertyName("max_value")]
    public object? MaxValue { get; }
    [JsonPropertyName("required")]
    public bool IsRequired { get; }
}