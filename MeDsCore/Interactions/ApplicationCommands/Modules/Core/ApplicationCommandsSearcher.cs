using System.Reflection;
using MeDsCore.Abstractions;
using MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core;

public class ApplicationCommandsSearcher
{
    private readonly ILogger _logger;

    private readonly ApplicationCommandBuilder _appCommandBuilder;
    private static readonly ApplicationCommandSignatureVerifier Verifier = new();
    
    public ApplicationCommandsSearcher(ILogger logger, bool isDebugMode, ulong debugGuildId)
    {
        _logger = logger;
        _appCommandBuilder = new ApplicationCommandBuilder(isDebugMode, debugGuildId);
    }

    public IEnumerable<ApplicationCommandInfo> SearchCommands(IEnumerable<Type> types)
    {
        var typesArray = types as Type[] ?? types.ToArray();
        if (!typesArray.Any()) yield break;

        foreach (var module in typesArray)
        {
            foreach (var methodInfo in module.GetMethods())
            {
                if (methodInfo.CustomAttributes.All(x =>
                        x.AttributeType.BaseType != typeof(ApplicationCommandAttribute)))
                {
                    continue;
                }
                
                if (VerifyCommand(methodInfo))
                {
                    yield return _appCommandBuilder.BuildInfo(methodInfo);
                }
            }
        }
    }

    private bool VerifyCommand(MethodInfo methodInfo)
    {
        var appCommandAttribute = methodInfo.GetCustomAttribute<ApplicationCommandAttribute>()!;

        var nameIsValid = Verifier.VerifyCommandName(appCommandAttribute.Name, appCommandAttribute.CommandType);
        var paramsAreValid = Verifier.VerifyCommandParameters(methodInfo, appCommandAttribute.CommandType);

        if (!nameIsValid)
        {
            _logger.LogWarning($"Invalid a command method signature {methodInfo.Name}. Name is invalid");
        }

        if (!paramsAreValid)
        {
            _logger.LogWarning($"Invalid a command method signature {methodInfo.Name}. Parameters are invalid");
        }
        
        return nameIsValid && paramsAreValid;
    }

    private void LogSkipCommand(string reason, string methodName)
    {
        _logger.LogWarning($"{methodName} method associated with command skipped. Reason: {reason}");
    }
}