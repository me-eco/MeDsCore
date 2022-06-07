namespace MeDsCore.Rest.Net;

/// <summary>
/// Результат использования меода Discord API 
/// </summary>
internal class DiscordMethodResult
{
    public DiscordMethodResult(bool isSuccess, Stream responseStream, DiscordError? error)
    {
        IsSuccess = isSuccess;
        ResponseStream = responseStream;
        Error = error;
    }
    
    /// <summary>
    /// True, если метод был выполнен успешно
    /// </summary>
    public bool IsSuccess { get; }
    /// <summary>
    /// Поток байтов ответа
    /// </summary>
    public Stream ResponseStream { get; }
    /// <summary>
    /// Ошибка
    /// </summary>
    public DiscordError? Error { get; }
}