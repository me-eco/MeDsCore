using Microsoft.Extensions.Logging;

namespace MeDsCore.Rest.Net;

public class DiscordDetailizedException : DiscordException
{
    /// <summary>
    /// Discord REST API Error, which fired the exception
    /// </summary>
    public DiscordError Error { get; }

    public DiscordDetailizedException(DiscordError error, string msg = "Discord API Error") : base(msg)
    {
        Error = error;
    }

    public DiscordDetailizedException(DiscordError error, ILogger logger, string msg = "Discord API Error") :
        this(error, msg)
    {
        logger.LogError($"ERROR CODE {error.Code} MESSAGE {msg}: {error.Message}");
    }
}