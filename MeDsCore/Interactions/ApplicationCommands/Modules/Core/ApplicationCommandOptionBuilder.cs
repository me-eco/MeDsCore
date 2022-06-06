using System.Reflection;
using MeDsCore.Abstractions;
using MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core;

/// <summary>
/// Фабрика для построения информации о параметре команды приложения типа CHAT_INPUT
/// </summary>
public class ApplicationCommandOptionBuilder
{
    private static readonly IReadOnlyDictionary<Type, ApplicationCommandOptionType> OptionTypes = new Dictionary<Type, ApplicationCommandOptionType>()
    {
        { typeof(int), ApplicationCommandOptionType.Integer },
        { typeof(double), ApplicationCommandOptionType.Number },
        { typeof(IDiscordChannel), ApplicationCommandOptionType.Channel },
        { typeof(IDiscordUser), ApplicationCommandOptionType.User },
        { typeof(IDiscordRole), ApplicationCommandOptionType.Role },
        { typeof(string), ApplicationCommandOptionType.String },
        { typeof(bool), ApplicationCommandOptionType.Boolean }
    }; // Все поддерживаемые типы параметров

    public OptionInfo BuildOption(ParameterInfo info)
    {
        var parameterType = info.ParameterType;
        var underliningNullType = Nullable.GetUnderlyingType(parameterType);
        var canBeNull = false;
        
        if (underliningNullType != null) // Если параметр может быть null
        {
            parameterType = underliningNullType;
            canBeNull = true;
        }
        
        if (!OptionTypes.ContainsKey(parameterType))
        {
            throw new InvalidOperationException("Failed to build unknown the option's type");
        }

        var optionType = OptionTypes[parameterType];
        var optionInfoAttribute = info.GetCustomAttribute<OptionAttribute>()!;

        var choiceAttrs = info.GetCustomAttributes<OptionChoiceAttribute>().ToArray();

        if (!choiceAttrs.Any())
        {
            return new OptionInfo(optionType, optionInfoAttribute.Name, optionInfoAttribute.Description,
                null, optionInfoAttribute.ChannelsTypes, optionInfoAttribute.MinValue,
                optionInfoAttribute.MaxValue, !canBeNull); 
        }

        var choices = new OptionChoice[choiceAttrs.Length]; 
        
        for (var i = 0; i < choices.Length; i++)
        {
            var choiceMeta = choiceAttrs[i];
            choices[i] = new OptionChoice(choiceMeta.Name, choiceMeta.Value);
        }
        
        return new OptionInfo(optionType, optionInfoAttribute.Name, optionInfoAttribute.Description,
            choices, optionInfoAttribute.ChannelsTypes, optionInfoAttribute.MinValue,
            optionInfoAttribute.MaxValue, !canBeNull); 
    }
}