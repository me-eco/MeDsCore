using System.Reflection;
using MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core;

/// <summary>
/// Фабрика для построения информации о команде приложения
/// </summary>
public class ApplicationCommandBuilder
{
    private readonly bool _isDebug;
    private readonly ulong _debugGuildId;
    private static readonly ApplicationCommandOptionBuilder AppCommandOptionBuilder = new();

    public ApplicationCommandBuilder(bool isDebug, ulong debugGuildId)
    {
        _isDebug = isDebug;
        _debugGuildId = debugGuildId;
    }
    
    public ApplicationCommandInfo BuildInfo(MethodInfo info)
    {
        //Атрибут, который показывает, что данная команда - команда приложения
        var applicationCommandAttr = info.GetCustomAttribute<ApplicationCommandAttribute>(); 
        var targetGuild = info.GetCustomAttribute<TargetGuildAttribute>(); // Если он null, то команда глобальная
        var commandPerm = info.GetCustomAttribute<CommandPermissionsAttribute>();
        var perms = commandPerm?.Permissions.ToString() ?? null;
        
        if (applicationCommandAttr == null)
        {
            throw new InvalidOperationException(
                "Failed to build application command information because the metadata attribute was NULL");
        }
        
        //Изначально аллоцируем пустой массив для экономии памяти
        var chatInputCommandOptions = Array.Empty<OptionInfo>();  
        ApplicationCommandType commandType;

        switch (applicationCommandAttr)
        {
            case SlashCommandAttribute: // Это единественный тип команд, где есть параметры
            {
                commandType = ApplicationCommandType.ChatInput;
                var parametersInfos = info.GetParameters();
                chatInputCommandOptions = new OptionInfo[parametersInfos.Length]; // Аллоциурем новый массив под информацию о параметрах 
            
                for (var i = 0; i < chatInputCommandOptions.Length; i++)
                {
                    var paramInfo = parametersInfos[i];
                    chatInputCommandOptions[i] = AppCommandOptionBuilder.BuildOption(paramInfo);
                }

                break;
            }
            case MessageCommandAttribute:
                commandType = ApplicationCommandType.Message;
                break;
            case UserCommandAttribute:
                commandType = ApplicationCommandType.User;
                break;
            default:
                throw new CustomAttributeFormatException("Invalid application command identifier: " + applicationCommandAttr.GetType().Name);
        }

        if (targetGuild != null || _isDebug) //Если команда приватна или включен режим отладки
        {
            switch (commandType)
            {
                case ApplicationCommandType.Message:
                    return new PrivateMessageCommandInfo(applicationCommandAttr.Name, info, _isDebug ? _debugGuildId : targetGuild!.Id, perms);
                case ApplicationCommandType.User:
                    return new PrivateUserCommandInfo(applicationCommandAttr.Name, info, _isDebug ? _debugGuildId : targetGuild!.Id, perms);
                case ApplicationCommandType.ChatInput:
                    return new PrivateChatInputCommandInfo(applicationCommandAttr.Name,
                        applicationCommandAttr!.Description!, info,
                        _isDebug ? _debugGuildId : targetGuild!.Id, chatInputCommandOptions, perms);
            }
        }
        else
        {
            switch (commandType)
            {
                case ApplicationCommandType.ChatInput:
                    return new GlobalChatInputCommandInfo(applicationCommandAttr.Name,
                        applicationCommandAttr!.Description!, info,
                        applicationCommandAttr.DmPermission, chatInputCommandOptions, perms);
                case ApplicationCommandType.User:
                    return new GlobalUserCommandInfo(applicationCommandAttr.Name, info, applicationCommandAttr.DmPermission, perms);
                case ApplicationCommandType.Message:
                    return new GlobalMessageCommandInfo(applicationCommandAttr.Name, info, applicationCommandAttr.DmPermission, perms);
            }
        }

        throw new InvalidOperationException("Failed to build a command");
    }
}