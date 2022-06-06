using MeDsCore.Abstractions;

namespace MeDsCore.Interactions.ApplicationCommands.Modules;

/// <summary>
/// Base class of all application commands modules
/// </summary>
public abstract class ApplicationCommandModule
{
    /// <summary>
    /// Guild where the command was received
    /// </summary>
    public IDiscordGuild ContextGuild { get; internal set; }
    /// <summary>
    /// User, which used the command
    /// </summary>
    public IDiscordUser Issuer { get; internal set; }
    /// <summary>
    /// Context socket, which contains tools for working with Application command
    /// </summary>
    public ApplicationCommandSocket ContextSocket { get; internal set; }
    
}