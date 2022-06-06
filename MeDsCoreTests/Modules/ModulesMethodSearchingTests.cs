using System.Linq;
using MeDsCore.Engine.Execution;
using Xunit;

namespace MeDsCoreTests.Modules;

public class ModulesMethodSearchingTests
{
    [Fact]
    public void SearchingByQuery()
    {
        var classInstance = new ClassA();
        var methodSearcher = new ModuleSocket(classInstance);

        ModuleMethod? moduleMethod = methodSearcher.Search(x => x.Name == "A");
        Assert.NotNull(moduleMethod);
        Assert.Equal(moduleMethod?.MethodName, "A");
        Assert.Equal(0, moduleMethod!.Params.Count());
    }

    private class ClassA : ModuleBase
    {
        public void A()
        {
            
        }
    }
}