using System.Net.Http.Json;
using MeDsCore.Abstractions;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Proxy;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Rest.Net;

/// <summary>
/// Выполняет методы Discord API 
/// </summary>
public class DiscordMethodExecutor : IMethodExecutor
{
    private readonly IDiscordProxy _proxy;
    private readonly AuthorizationProvider _authProvider;
    private readonly ILogger _logger;

    /// <summary>
    /// Инитиализирвует новый объект
    /// </summary>
    /// <param name="authProvider">Провайдер аунтефикационного токена</param>
    /// <param name="proxy">Абстракция для использования Discord API</param>
    public DiscordMethodExecutor(IDiscordProxy proxy, AuthorizationProvider authProvider, ILogger logger)
    {
        _proxy = proxy;
        _authProvider = authProvider;
        _logger = logger;
    }
    
    public async Task<DiscordMethodResult> ExecuteMethodAsync(DiscordMethodInfo methodConfiguration, IContentBuilder contentBuilder)
    {
        var httpRequest = methodConfiguration.ConfigureScaffoldHttpRequest();

        contentBuilder.CreateContent(httpRequest);
        _authProvider.CreateAuth(httpRequest);
        
        var discordResponse = await _proxy.SendAsync(httpRequest);

        DiscordError? error = null;

        if (!discordResponse.IsSuccessStatusCode)
        {
            error = await discordResponse.Content.ReadFromJsonAsync<DiscordError>();
        }

        _logger.LogDebug(
            $"<{httpRequest.Method.ToString().ToUpper()}> {httpRequest.RequestUri!.AbsolutePath}; Success: {discordResponse.IsSuccessStatusCode}; Code: {(int)discordResponse.StatusCode}");
        
        return new DiscordMethodResult(discordResponse.IsSuccessStatusCode, await discordResponse.Content.ReadAsStreamAsync(),
            error);
    }
}