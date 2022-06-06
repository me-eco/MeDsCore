using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MeDsCore.Engine.Reflection;

public class AssemblyModuleTypesLoader
{
    private readonly ModulesFilter _filter;

    public AssemblyModuleTypesLoader(Type commandModuleType)
    {
        _filter = new ModulesFilter(commandModuleType);
    }

    public IEnumerable<Type> LoadModules(Assembly assembly)
    {
        return _filter.Filter(assembly.GetTypes());
    }

    public void LoadModulesIntoServices(IServiceCollection serviceCollection, Assembly targetAssembly)
    {       
        var modules = LoadModules(targetAssembly);

        foreach (var module in modules)
        {
            serviceCollection.AddTransient(module);
        }
    }
}