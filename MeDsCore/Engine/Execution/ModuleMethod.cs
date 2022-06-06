namespace MeDsCore.Engine.Execution;

/// <summary>
/// Lightweight execution provider for a method from context module  
/// </summary>
public class ModuleMethod
{
    private readonly InvocationDelegate _invocationDelegate;

    public delegate void InvocationDelegate(object[] args);
    
    /// <summary>
    /// Initializes new instance of <see cref="ModuleMethod"/>
    /// </summary>
    /// <param name="methodName">Name of the module</param>
    /// <param name="params">Parameters info-s</param>
    /// <param name="invocationDelegate">Delegate, which allows invoke method</param>
    public ModuleMethod(string methodName, IEnumerable<ParamInfo> @params, InvocationDelegate invocationDelegate)
    {
        _invocationDelegate = invocationDelegate;
        MethodName = methodName;
        Params = @params;
    }

    public string MethodName { get; }
    public IEnumerable<ParamInfo> Params { get; }

    public void Execute(object[] args) => _invocationDelegate(args);
}

/// <summary>
/// Lightweight parameter information
/// </summary>
public struct ParamInfo
{
    public ParamInfo(string paramName, Type paramType)
    {
        ParamName = paramName;
        ParamType = paramType;
    }

    public string ParamName { get; }
    public Type ParamType { get; }
}