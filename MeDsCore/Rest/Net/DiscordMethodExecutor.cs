using System.Net;
using System.Net.Http.Json;
using MeDsCore.Rest.Entities;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Proxy;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Rest.Net;

/// <summary>
/// Executesn Discord REST API's methods 
/// </summary>
internal class DiscordMethodExecutor : IMethodExecutor
{
    private readonly IDiscordProxy _proxy;
    private readonly AuthorizationProvider _authProvider;
    private readonly ILogger _logger;
    
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
        var absPath = httpRequest.RequestUri!.AbsolutePath;
        
        _logger.LogDebug(
            "<{Method}> {AbsPath}; Success: {IsSuccess}; Code: {StatusCode}", 
            httpRequest.Method.ToString().ToUpper(), absPath,
            discordResponse.IsSuccessStatusCode, (int)discordResponse.StatusCode);
        
        if (discordResponse.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogDebug("Rate limit for {Path} exceeded", absPath);
            return await ReexcuteMethodWithForceAsync(httpRequest, discordResponse);
        }
        
        return await ContructMethodResultAsync(discordResponse);
    }

    /// <summary>
    /// Executes a Discord REST API method while server response with the HTTP 429 Status Code 
    /// </summary>
    /// <param name="message">Message to execute</param>
    /// <returns>Forced the Discord REST API Method execution result</returns>
    private async Task<DiscordMethodResult> ExecuteMethodForceAsync(HttpRequestMessage message)
    {
        HttpResponseMessage discordResponse = await _proxy.SendAsync(message);
        
        _logger.LogDebug("<{Method}> FORCE {AbsPath}; Success: {IsSuccess}; Code: {StatusCode}",
            message.Method.ToString().ToUpper(), message.RequestUri!.AbsolutePath, discordResponse.IsSuccessStatusCode, (int)discordResponse.StatusCode);

        if (discordResponse.StatusCode == HttpStatusCode.TooManyRequests)
        {
            return await ReexcuteMethodWithForceAsync(message, discordResponse);
        }

        return await ContructMethodResultAsync(discordResponse);
    }

    /// <summary>
    /// Reexecutes the REST API Method with delay, which declared in a server's response
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="invokerMessage">Message with a HTTP 429 Status Code</param>
    /// <returns>Forced the Discord REST API Method execution result</returns>
    private async Task<DiscordMethodResult> ReexcuteMethodWithForceAsync(HttpRequestMessage message, HttpResponseMessage invokerMessage)
    {
        var tooManyRequestsEntity = await invokerMessage.Content!.ReadFromJsonAsync<TooManyRequestsEntity>();

        await Task.Delay(TimeSpan.FromSeconds(tooManyRequestsEntity!.RetryAfter));
        return await ExecuteMethodForceAsync(message);
    }

    private static async Task<DiscordMethodResult> ContructMethodResultAsync(HttpResponseMessage discordResponse)
    {
        var error = await discordResponse.Content.ReadFromJsonAsync<DiscordError>();

        return new DiscordMethodResult(discordResponse.IsSuccessStatusCode,
            await discordResponse.Content.ReadAsStreamAsync(), error);
    }
}