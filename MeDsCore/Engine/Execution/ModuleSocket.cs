using System.Reflection;

namespace MeDsCore.Engine.Execution;

/// <summary>
/// Socket allows you to create manipulations with mdule
/// </summary>
public class ModuleSocket
{
    private readonly ModuleBase _module;

    public ModuleSocket(ModuleBase module)
    {
        _module = module;
    }

    /// <summary>
    /// Searchs a method in module, which meets condition <see cref="query"/>
    /// </summary>
    /// <param name="query">Condition for searching method</param>
    public ModuleMethod? Search(Func<MethodInfo, bool> query)
    {
        var methods = _module.GetType().GetMethods();

        foreach (var methodInfo in methods)
        { 
            if (!query(methodInfo)) continue;
                    
            var parametres = methodInfo.GetParameters();
            var methodParams = from param in parametres
                select new ParamInfo(param.Name, param.ParameterType);
            var invocator = new ModuleMethod.InvocationDelegate(x =>
            {
                methodInfo.Invoke(this, x);
            });
            
            return new ModuleMethod(methodInfo.Name, methodParams, invocator);
        }

        return null;
    }
}