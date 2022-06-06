namespace MeDsCore.Rest.Net;

/// <summary>
/// An error, which received from Discord REST API
/// </summary>
public class DiscordError
{
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int Code { get; init; }
    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; init; }
}
