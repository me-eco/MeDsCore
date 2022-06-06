namespace MeDsCore.Rest.Net;

/// <summary>
/// Ошибка, которая была получена от Discord API
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
