using MeDsCore.Abstractions;
using MeDsCore.Interactions.ApplicationCommands.Modules;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest;
using Microsoft.Extensions.DependencyInjection;
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
    /// <param name="appCommandContext">Сокет команды</param>
    /// <exception cref="InvalidOperationException">Вызывается, если выполнение команды провалилось</exception>
    public async Task ExecuteCommandAsync(ApplicationCommandContext appCommandContext)
    {
        var commandId = appCommandContext.InteractionResponse.Data.Id;
        var exeInfo = ValidateCommandSignature(commandId); // Валидация команды по id
        var methodInfo = exeInfo.ReflectionMethodInfo; // Данные о методе, полученные с помощь рефлексии
        var moduleType = methodInfo.DeclaringType!; // Тип модуля, в котором содержится команда
        
        if (moduleType.BaseType! != typeof(ApplicationCommandModule))
        {
            ThrowFailedToExecuteCommand();
        }
        
        var constructorOfModule = moduleType.GetConstructors()[0]; // Единственный конструктор (гарантируется, что он такой один)
        var parameters = constructorOfModule.GetParameters(); // Параметры конструктора
        var argsForCtor = new object[parameters.Length];
        using var serviceScope = _services.CreateScope();
        
        for (var i = 0; i < parameters.Length; i++)
        {
            argsForCtor[i] = serviceScope.ServiceProvider.GetService(parameters[i].ParameterType)!;
        }

        var moduleInstance = constructorOfModule.Invoke(argsForCtor);
        var commandModule = (ApplicationCommandModule)moduleInstance;
        
        await FillCommandModuleAsync(appCommandContext, commandModule);

        var commandReflectedParameters = methodInfo.GetParameters();
        var commandParameters = new object?[commandReflectedParameters.Length];
        var argsArray = appCommandContext.Args.ToArray();
        
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

        var invocationResult = methodInfo.Invoke(commandModule, commandParameters);

        if (invocationResult is Task commandTask)
        {
            await commandTask;
        }
        
        GC.Collect(0);
    }

    private ApplicationCommandInfo ValidateCommandSignature(ulong commandId)
    {
        var containsCommandInRegistry = _registry.TryGetValue(commandId, out var appCommandExecutionInfo);

        if (!containsCommandInRegistry)
        {
            ThrowFailedToExecuteCommand("Unknown command");
        }
        if(appCommandExecutionInfo == null) ThrowFailedToExecuteCommand();
        var commandDislocation = appCommandExecutionInfo!.ReflectionMethodInfo.DeclaringType;
        if(commandDislocation == null) ThrowFailedToExecuteCommand();
        
        return appCommandExecutionInfo;
    }

    private async Task FillCommandModuleAsync(ApplicationCommandContext context, ApplicationCommandModule module)
    {
        var ctxGuildId = context.InteractionResponse.GuildId;
        var ctxGuild = (await _restClient.GetGuildAsync(ctxGuildId))!;

        module.Issuer = context.InteractionResponse.Member ?? context.InteractionResponse.User;
        module.Guild = ctxGuild;
        module.Context = context;
    }
    
    private void ThrowFailedToExecuteCommand(string msg = "Failed to execute application command")
    {
        throw new DiscordException(msg, _logger);
    }
}
