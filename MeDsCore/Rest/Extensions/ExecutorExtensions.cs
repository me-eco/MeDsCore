using System.Text.Json;
using MeDsCore.Abstractions;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Rest.Extensions;

internal static class ExecutorExtensions
{
    /// <summary>
    /// Executes method without any HTTP Content
    /// </summary>
    /// <param name="methodExecutor">A method executor abstraction</param>
    /// <param name="info">Meta information about a method</param>
    /// <returns>REST API Response</returns>
    public static Task<DiscordMethodResult> ExecuteMethodAsync(this IMethodExecutor methodExecutor, DiscordMethodInfo info)
    {
        return methodExecutor.ExecuteMethodAsync(info, IContentBuilder.EmptyContent);
    }

    /// <summary>
    /// Executes method
    /// </summary>
    /// <param name="methodExecutor">A method executor abstraction</param>
    /// <param name="info">Meta information about a method</param>
    /// <param name="builder">HTTP Content builder</param>
    /// <param name="ensureSuccess">If true and a request failed throws <see cref="DiscordException"/></param>
    /// <param name="logger">Abstraction wich provides methods for logging</param>
    /// <returns>REST API entity</returns>
    public static async Task<T?> ExecuteMethodAsync<T>(this IMethodExecutor methodExecutor, 
        DiscordMethodInfo info, IContentBuilder builder, bool ensureSuccess = true, ILogger? logger = null) where T : class
    {
        var result = await methodExecutor.ExecuteMethodAsync(info, builder);

        if (ensureSuccess && !result.IsSuccess)
        {
            if (result.Error != null)
            {
                if(logger != null) throw new DiscordDetailizedException(result.Error, logger);
                throw new DiscordDetailizedException(result.Error);
            }
            
            if (logger != null) throw new DiscordException("Failed to execute the REST API method", logger);
            throw new DiscordException("Failed to execute the REST API method");
        }

        if (result.IsSuccess) return await JsonSerializer.DeserializeAsync<T>(result.ResponseStream);
        
        logger?.LogDebug("Failed to execute the REST API method");
        return null;

    }

    /// <summary>
    /// Executes method without any HTTP Content
    /// </summary>
    /// <param name="methodExecutor">A method executor abstraction</param>
    /// <param name="info">Meta information about a method</param>
    /// <param name="ensureSuccess">If true and a request failed throws <see cref="DiscordException"/></param>
    /// /// <param name="logger">Abstraction wich provides methods for logging</param>
    /// <returns>REST API entity</returns>
    public static Task<T?> ExecuteMethodAsync<T>(this IMethodExecutor methodExecutor,
        DiscordMethodInfo info, bool ensureSuccess = true, ILogger? logger = null) where T : class =>
            ExecuteMethodAsync<T>(methodExecutor, info, IContentBuilder.EmptyContent, ensureSuccess, logger);
}