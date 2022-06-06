using System.Text.Json.Serialization;
using MeDsCore.Base;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules;

/// <summary>
/// Содержит информацию о параметре команды
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

    /// <summary>
    /// Тип параметра
    /// </summary>
    [JsonPropertyName("type")]
    public ApplicationCommandOptionType Type { get; }
    /// <summary>
    /// Название параметра
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }
    /// <summary>
    /// Описание
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; }
    /// <summary>
    /// Возможные шаблоны праметра
    /// </summary>
    [JsonPropertyName("choices")]
    public OptionChoice[]? Choices { get; }
    /// <summary>
    /// Создержит допустимые типы каналов
    /// </summary>
    [JsonPropertyName("channels_types")]
    public ChannelType[]? ChannelsTypes { get; }
    /// <summary>
    /// Минимальное значение (Если <see cref="Type"/> имеет значение <see cref="ApplicationCommandOptionType.Integer"/>, то можно привести к типу <see cref="int"/>. Также это относится к вещественным числам)
    /// </summary>
    [JsonPropertyName("min_value")]
    public object? MinValue { get; }
    /// <summary>
    /// Максимальное значение (Если <see cref="Type"/> имеет значение <see cref="ApplicationCommandOptionType.Integer"/>, то можно привести к типу <see cref="int"/>. Также это относится к вещественным числам)
    /// </summary>
    [JsonPropertyName("max_value")]
    public object? MaxValue { get; }
    /// <summary>
    /// Если true, то параметр необязателен
    /// </summary>
    [JsonPropertyName("required")]
    public bool IsRequired { get; }
}