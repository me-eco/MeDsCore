using System.Reflection;
using MeDsCore.Abstractions;
using MeDsCore.Interactions.ApplicationCommands.Modules;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;
using MeDsCore.Interactions.Base;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest;
using MeDsCore.Rest.Extensions;
using MeDsCore.Rest.Net;
using MeDsCore.WebSocket;
using MeDsCore.WebSocket.Gateway;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Interactions.ApplicationCommands;

/// <summary>
/// Клиент для работы с ApplicationCommands. Должен быть инициализирован после Ready события
/// </summary>
public class ApplicationCommandsClient : IGatewayMessageHandler
{
    private readonly IMethodExecutor _methodExecutor;
    private readonly ulong _applicationId;
    private readonly ILogger _logger;
    private readonly IServiceProvider _services;
    private readonly ApplicationCommandsRegistrator _appCommandsRegistrator;
    private readonly ApplicationCommandsExecutor _applicationCommandsExecutor;
    private readonly ApplicationCommandsSearcher _applicationCommandsSearcher;
    private readonly Dictionary<ulong, ApplicationCommandInfo> _appCommandsRegistry = new();
    private readonly ICollection<Type> _modulesRegistry = new List<Type>();

    private static readonly Type AppCommandModuleType = typeof(ApplicationCommandModule);

    public ApplicationCommandsClient(DiscordRestClient restClient,
        DiscordWebSocketClient webSocketClient,
        ILogger logger,
        IServiceProvider services,
        bool isDebugMode = false, ulong debugGuildId = 0)
    {
        if (!webSocketClient.IsReady)
        {
            throw new InvalidOperationException("WS client must be READY to initialize new application commands client");
        }
        
        _applicationId = webSocketClient.ApplicationId!.Value;
        _logger = logger;
        _services = services;
        _methodExecutor = restClient.MethodExecutor;
        _applicationCommandsSearcher = new ApplicationCommandsSearcher(logger, isDebugMode, debugGuildId);
        _appCommandsRegistrator = new ApplicationCommandsRegistrator(_applicationId, _methodExecutor);
        _applicationCommandsExecutor = new ApplicationCommandsExecutor(_appCommandsRegistry, services, logger, restClient);
    }

    public event Func<ApplicationCommandSocket, Task>? OnApplicationCommandReceived;
    /// <summary>
    /// Список всех команд (только для чтения)
    /// </summary>
    public IReadOnlyDictionary<ulong, ApplicationCommandInfo> ApplicationCommandsRegistry => _appCommandsRegistry;

    /// <summary>
    /// Получает приватную команду
    /// </summary>
    /// <param name="guildId">Сервер, где находится команда</param>
    /// <returns>Перечисление всех команд</returns>
    public async Task<IEnumerable<ApplicationCommand>> GetPrivateApplicationCommandsAsync(ulong guildId)
    {
        var getPrivateConfig = ApplicationCommandsMethods.ConfigureGetPrivateApplicationCommands(_applicationId, guildId);
        var commandsEntities = await _methodExecutor.ExecuteMethodAsync<ApplicationCommandEntity[]>(getPrivateConfig);

        return from entity in commandsEntities select new ApplicationCommand(entity);
    }

    /// <summary>
    /// Добавляет командный модуль
    /// </summary>
    /// <param name="moduleType">Тип модуля. Должен обязательно наследовать <see cref="ApplicationCommandModule"/></param>
    public void AddModule(Type moduleType)
    {
        var constructors = moduleType.GetConstructors();

        if (moduleType.BaseType != AppCommandModuleType)
        {
            throw new InvalidOperationException("Application command module must inherits ApplicationCommandModule");
        }
        
        if (constructors.Length > 1)
        {
            throw new InvalidOperationException("Application command module must has one constructor");
        }

        var constructor = constructors[0];
        var ctorParams = constructor.GetParameters();

        if (ctorParams.Length > 0)
        {
            foreach (var parameter in ctorParams)
            {
                if (_services.GetService(parameter.ParameterType) == null)
                {
                    throw new InvalidOperationException($"Invalid type of the constructor's parameter: {parameter.ParameterType}");
                }
            }  
        }

        _modulesRegistry.Add(moduleType);
    }

    public void AddModule<T>() where T : class => AddModule(typeof(T));

    public void AddAssemblyModules(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if(type.BaseType == typeof(ApplicationCommandModule)) AddModule(type);
        }
    }
    
    /// <summary>
    /// Выполняет регистрацию модулей и привязывает их к <see cref="ApplicationCommandsExecutor"/>, который запускает команды
    /// </summary>
    public async Task StartModulesAsync()
    {
        var commands = _applicationCommandsSearcher.SearchCommands(_modulesRegistry);
        
        foreach (var commandInfo in commands)
        {
            var commandId = await _appCommandsRegistrator.CreateCommandAsync(commandInfo);
            _appCommandsRegistry.Add(commandId, commandInfo);
        }
    }
    
    /// <summary>
    /// Получает все глобальные команды 
    /// </summary>
    public async Task<IEnumerable<ApplicationCommand>> GetGlobalApplicationCommandsAsync()
    {
        var getGlobCommandsConfig = ApplicationCommandsMethods.ConfigureGetGlobalApplicationCommands(_applicationId);
        var commandsEntities = await _methodExecutor.ExecuteMethodAsync<ApplicationCommandEntity[]>(getGlobCommandsConfig);

        return from entity in commandsEntities select new ApplicationCommand(entity);
    }

    async Task IGatewayMessageHandler.ProcessMessageAsync(DiscordGatewayMessage message)
    {
        if(message.Opcode != GatewayServerOpcode.Dispatch) return;
        if(message.EventName != "INTERACTION_CREATE") return;
        if(message.Content == null) throw new DiscordException("Expected interaction entity but it was null");
        
        var interactionEntity = message.ConvertJson<InteractionBaseEntity>();

        _logger.LogTrace($"New interaction received. Type: {interactionEntity.Type.ToString().ToUpper()}");

        if (interactionEntity.Type == InteractionType.ApplicationCommand)
        {
            var interBase = await ApplicationCommandInteraction.InitializeAsync(interactionEntity, _methodExecutor);
            var socket = new ApplicationCommandSocket(_methodExecutor, interBase);
            OnApplicationCommandReceived?.Invoke(socket);
            if(_appCommandsRegistry.Count > 0) await _applicationCommandsExecutor.ExecuteCommandAsync(socket);
        }
    }
}