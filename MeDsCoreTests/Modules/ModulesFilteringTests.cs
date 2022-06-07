using System;
using System.Collections.Generic;
using System.Linq;
using MeDsCore.Engine;
using Xunit;

namespace MeDsCoreTests.Modules;

public class ModulesFilteringTests
{
    [Theory]
    [InlineData(new[] {typeof(ClassA), typeof(ClassB)}, new[] {typeof(ClassA)})]
    [InlineData(new[] {typeof(ClassB), typeof(ClassB)}, new Type[0])]
    [InlineData(new[] {typeof(ClassA), typeof(ClassA)}, new[] {typeof(ClassA)})]
    public void FilterCommands_Test(Type[] types, Type[] commandsTypes)
    {
        var commandModuleType = typeof(TestBaseModule);
        var commandModulesFilter = new ModulesFilter(commandModuleType);
        IEnumerable<Type> commandModules = commandModulesFilter.Filter(types);

        var collection = commandModules as Type[] ?? commandModules.ToArray();
        if (commandsTypes.Length == 0)
        {
            Assert.Empty(collection);
        }
        
        Assert.True(collection.All(x => commandsTypes.Any(z => z == x)));
    }

    private class TestBaseModule {}
    
    private class ClassA : TestBaseModule { }
    
    private class ClassB {}
}