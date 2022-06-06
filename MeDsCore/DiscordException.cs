using MeDsCore.Abstractions;
using Microsoft.Extensions.Logging;

namespace MeDsCore;

public class DiscordException : SystemException
{
    public DiscordException(string msg = "Ошибка Discord") : base(msg)
    {
        
    }

    public DiscordException(string msg, ILogger logger) : this(msg)
    {
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        logger.LogCritical(msg);
    }
}