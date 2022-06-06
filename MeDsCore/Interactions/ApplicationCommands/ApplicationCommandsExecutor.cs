using MeDsCore.Abstractions;
using MeDsCore.Interactions.ApplicationCommands.Modules;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Interactions.ApplicationCommands;

/// <summary>
/// Позволяет выполнять команды приложения
/// </summary>
internal class ApplicationCommandsExecutor
{
    private readonly IReadOnlyDictionary<ulong, ApplicationCommandInfo> _registry;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;
    private readonly DiscordRestClient _restClient;

    public ApplicationCommandsExecutor(IReadOnlyDictionary<ulong, ApplicationCommandInfo> registry,
        IServiceProvider services,
        ILogger logger,
        DiscordRestClient restClient)
    {
        _registry = registry;
        _services = services;
        _logger = logger;
        _restClient = restClient;
    }

    /// <summary>
    /// Выполняет метод
    /// </summary>
    /// <param name="appCommandSocket">Сокет команды</param>
    /// <exception cref="InvalidOperationException">Вызывается, если выполнение команды провалилось</exception>
    public async Task ExecuteCommandAsync(ApplicationCommandSocket appCommandSocket)
    {
        var appCommand = appCommandSocket.InteractionResponse.Data; // Данные об команде приложения
        var commandId = appCommand.Id;
        var exeInfo = ValidateCommandSignature(commandId); // Валидация команды по id
        var methodInfo = exeInfo.ReflectionMethodInfo; // Данные о методе, полученные с помощь рефлексии
        var moduleType = methodInfo.DeclaringType!; // Тип модуля, в котором содержится команда
        var constructorOfModule = moduleType.GetConstructors()[0]; // Единственный конструктор (гарантируется, что он такой один)
        var parameters = constructorOfModule.GetParameters(); // Параметры конструктора
        var argsForCtor = new object[parameters.Length];
        
        for (var i = 0; i < parameters.Length; i++)
        {
            argsForCtor[i] = _services.GetService(parameters[i].ParameterType)!;
        }

        var moduleInstance = constructorOfModule.Invoke(argsForCtor);

        if (moduleInstance is not ApplicationCommandModule)
        {
            ThrowFailedToExecuteCommand();
        }
        var commandModule = (ApplicationCommandModule)moduleInstance;
        
        await FillCommandModuleAsync(appCommandSocket, commandModule);

        var commandReflectedParameters = methodInfo.GetParameters();
        var commandParameters = new object?[commandReflectedParameters.Length];
        var argsArray = appCommandSocket.Args.ToArray();
        
        for (var i = 0; i < commandParameters.Length; i++) //Преобразовываем объекты, полученные от Discord Gateway в аргументы
        {
            var arg = argsArray[i];
            commandParameters[i] = arg.Type switch
            {
                ApplicationCommandOptionType.String => arg.Value.ToString(),
                ApplicationCommandOptionType.Integer => (int)arg.Value,
                ApplicationCommandOptionType.Boolean => (bool)arg.Value,
                ApplicationCommandOptionType.Number => (double)arg.Value,
                ApplicationCommandOptionType.User => (IDiscordUser)arg.Value,
                ApplicationCommandOptionType.Channel => (IDiscordChannel)arg.Value,
                ApplicationCommandOptionType.Role => (IDiscordRole)arg.Value,
                _ => throw new InvalidOperationException("Failed to fill parameter")
            };
        }

        methodInfo.Invoke(commandModule, commandParameters);
        GC.Collect(0); //Очищаем нулевую генерацию мусора, чтобы удалить объекты, которые были созданы в процессе выполнения команды
    }

    private ApplicationCommandInfo ValidateCommandSignature(ulong commandId)
    {
        var containsCommandInRegistry = _registry.TryGetValue(commandId, out var appCommandExecutionInfo);
        _logger.LogTrace($"Preparing for execution application command ({commandId}) started");

        if (!containsCommandInRegistry)
        {
            ThrowFailedToExecuteCommand("Unknown command");
        }
        if(appCommandExecutionInfo == null) ThrowFailedToExecuteCommand();
        var commandDislocation = appCommandExecutionInfo!.ReflectionMethodInfo.DeclaringType;
        if(commandDislocation == null) ThrowFailedToExecuteCommand();
        
        return appCommandExecutionInfo;
    }

    private async Task FillCommandModuleAsync(ApplicationCommandSocket socket, ApplicationCommandModule module)
    {
        var ctxGuildId = socket.InteractionResponse.GuildId;
        var ctxGuild = (await _restClient.GetGuildAsync(ctxGuildId))!;

        module.Issuer = socket.InteractionResponse.Member ?? socket.InteractionResponse.User;
        module.ContextGuild = ctxGuild;
        module.ContextSocket = socket;
    }
    
    private void ThrowFailedToExecuteCommand(string msg = "Failed to execute application command")
    {
        throw new DiscordException(msg, _logger);
    }
}