namespace MeDsCore.Engine;

public class ModulesFilter
{
    private readonly Type _commandModuleType;

    public ModulesFilter(Type commandModuleType)
    {
        _commandModuleType = commandModuleType;
    }

    public IEnumerable<Type> Filter(IEnumerable<Type> types)
    {
        return types.Where(IsCommandModule);
    }
    
    private bool IsCommandModule(Type type)
    {
        if (type.BaseType == null) return false;
        return type.BaseType == _commandModuleType;
    }
}