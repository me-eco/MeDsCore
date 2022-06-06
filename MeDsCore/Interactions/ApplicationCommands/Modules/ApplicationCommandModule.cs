using MeDsCore.Abstractions;

namespace MeDsCore.Interactions.ApplicationCommands.Modules;

/// <summary>
/// Base class of all application commands modules
/// </summary>
public abstract class ApplicationCommandModule
{
    /// <summary>
    /// Сервер, откуда была получена команда
    /// </summary>
    public IDiscordGuild ContextGuild { get; internal set; }
    /// <summary>
    /// Пользователь, который использовал команду
    /// </summary>
    public IDiscordUser Issuer { get; internal set; }
    /// <summary>
    /// Контекст команды, который содержит методы и свойства для работы с входящим Interaction
    /// </summary>
    public ApplicationCommandSocket ContextSocket { get; internal set; }
    
}