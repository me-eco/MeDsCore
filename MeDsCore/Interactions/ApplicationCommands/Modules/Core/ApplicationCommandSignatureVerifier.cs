using System.Reflection;
using System.Text.RegularExpressions;
using MeDsCore.Abstractions;
using MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core;

public class ApplicationCommandSignatureVerifier
{
    private readonly Type[] _appCommandsOptionTypes = new[] { typeof(string), typeof(int), typeof(double), typeof(bool), typeof(IDiscordUser), typeof(IDiscordChannel), typeof(IDiscordRole) };

    public bool VerifyCommandName(string name, ApplicationCommandType commandType)
    {
        if (name.Length is < 1 or > 32) return false;
        if (commandType != ApplicationCommandType.ChatInput) return true;

        return VerifyAppCommandNamingRule(name);
    }

    private bool VerifyAppCommandNamingRule(string val) => Regex.IsMatch(val, @"^[\w-]{1,32}$");

    public bool VerifyCommandParameters(MethodInfo methodInfo, ApplicationCommandType commandType)
    {
        var parameterInfos = methodInfo.GetParameters();
        
        if (commandType != ApplicationCommandType.ChatInput && parameterInfos.Length != 0) return false;
        if(commandType != ApplicationCommandType.ChatInput) return true;

        foreach (var parameterInfo in parameterInfos)
        {
            var paramType = parameterInfo.ParameterType;
            var underliningNullType = Nullable.GetUnderlyingType(paramType);

            if (underliningNullType != null) //Проверка на nullable параметр
            {
                paramType = underliningNullType;
                //Требуем, что nullable параметр должен быть опционален
                if (!parameterInfo.IsOptional)
                {
                    return false;
                }
            }
            
            if (_appCommandsOptionTypes.All(x => paramType != x)) return false; //Проверка типа параметра
            
            //Затем проверим, если параметр имеет метаданные
            var optionInfoAttr = parameterInfo.GetCustomAttribute<OptionAttribute>();
            var choicesAttrs = parameterInfo.GetCustomAttributes<OptionChoiceAttribute>();
            if (optionInfoAttr == null) return false;

            bool CheckMinMaxWithTypeIsIncorrect(Type type)
            {
                return paramType != type &&
                       optionInfoAttr!.MaxValue != null && optionInfoAttr.MaxValue.GetType() != type &&
                       optionInfoAttr.MinValue != null && optionInfoAttr.MinValue.GetType() != type;
            }
            
            if (//Проверка на то, что может ли иметь тип шаблоны
                _appCommandsOptionTypes.All(x => x != paramType) && !choicesAttrs.Any() ||
                //Проверка на типы каналов
                paramType != typeof(IDiscordChannel) && optionInfoAttr.ChannelsTypes is {Length: > 0} ||
                //Проверка мин/макс значений
                CheckMinMaxWithTypeIsIncorrect(typeof(int)) || CheckMinMaxWithTypeIsIncorrect(typeof(double)))
            {
                return false;
            }
        }

        return true;
    }
}