using MeDsCore.Rest.Net.Content;

namespace MeDsCore.Rest.Net;

internal interface IMethodExecutor
{
    /// <summary>
    /// Выполняет методы API с помощью метаданных
    /// </summary>
    /// <param name="methodInfo">Метаданные метода</param>
    /// <param name="contentBuilder">HTTP контент, который нужно сгенерировать</param>
    /// <returns></returns>
    Task<DiscordMethodResult> ExecuteMethodAsync(DiscordMethodInfo methodInfo, IContentBuilder contentBuilder);
}