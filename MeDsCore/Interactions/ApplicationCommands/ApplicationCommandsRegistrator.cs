using System.Text.Json.Serialization;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;
using MeDsCore.Interactions.Base;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest.Extensions;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;

namespace MeDsCore.Interactions.ApplicationCommands;

internal class ApplicationCommandsRegistrator
{ 
    private readonly ulong _applicationId;
    private readonly IMethodExecutor _methodExecutor;
    
    public ApplicationCommandsRegistrator(ulong applicationId, IMethodExecutor methodExecutor)
    {
        _applicationId = applicationId;
        _methodExecutor = methodExecutor;
    }

    public async Task DeleteGlobalCommandAsync(ulong commandId)
    {
        var deleteMethodInfo = ApplicationCommandsMethods.ConfigureDeleteGlobalApplicationCommand(_applicationId, commandId);
        var result = await _methodExecutor.ExecuteMethodAsync(deleteMethodInfo);

        if (!result.IsSuccess)
        {
            throw new DiscordException($"Failed to delete global command ({commandId})");
        }
    }
    
    public async Task DeletePrivateCommandAsync(ulong guildId, ulong commandId)
    {
        var deleteMethodInfo = ApplicationCommandsMethods.ConfigureDeletePrivateApplicationCommand(_applicationId, guildId, commandId);
        var result = await _methodExecutor.ExecuteMethodAsync(deleteMethodInfo);

        if (!result.IsSuccess)
        {
            throw new DiscordException($"Failed to delete global command ({commandId})");
        }
    }

    public async Task EditGlobalCommandAsync(ulong commandId, GlobalApplicationCommandUpdate update)
    {
        var editGlobalCommandInfo = ApplicationCommandsMethods.ConfigureEditGlobalApplicationCommand(_applicationId, commandId);
        var jsonContent = new JsonContentBuilder(update);

        var result = await _methodExecutor.ExecuteMethodAsync(editGlobalCommandInfo, jsonContent);

        if (!result.IsSuccess) throw new DiscordException("Failed to edit global command");
    }
    
    public async Task EditPrivateCommandAsync(ulong guildId, ulong commandId, PrivateApplicationCommandUpdate update)
    {
        var editPrivateCommandInfo =
            ApplicationCommandsMethods.ConfigureEditPrivateApplicationCommand(_applicationId, guildId, commandId);
        var jsonContent = new JsonContentBuilder(update);

        var result = await _methodExecutor.ExecuteMethodAsync(editPrivateCommandInfo, jsonContent);

        if (!result.IsSuccess) throw new DiscordException("Failed to edit private command");
    }

    public async Task<ulong> CreateCommandAsync(ApplicationCommandInfo commandInfo)
    {
        DiscordMethodInfo restInfo;
        
        if(commandInfo is PrivateApplicationCommandInfo info)
        {
            restInfo = ApplicationCommandsMethods.ConfigureCreatePrivateApplicationCommand(_applicationId, info.GuildId);
        }
        else
        {
            restInfo = ApplicationCommandsMethods.ConfigureCreateGlobalApplicationCommand(_applicationId);
        }

        var appCommand = await _methodExecutor.ExecuteMethodAsync<ApplicationCommandEntity>(restInfo, commandInfo.ProvideContentBuilder());

        return ulong.Parse(appCommand!.Id);
    }
}

public class GlobalApplicationCommandUpdate
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("dm_permission")]
    public bool? DmPermission { get; set; }
    [JsonPropertyName("default_permission")]
    public bool? DefaultPermission { get; set; }
}

public class PrivateApplicationCommandUpdate
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("default_permission")]
    public bool? DefaultPermission { get; set; }
}