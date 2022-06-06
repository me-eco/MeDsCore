using System;
using MeDsCore.Abstractions;
using MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core;
using Xunit;

namespace MeDsCoreTests.Interactions.ApplicationCommandsModules;

public class BuildingOptionsTests
{
    [Fact]
    public void OptionBuildingTest()
    {
        var method = typeof(TestClassA).GetMethod("TestMethodA")!;
        var optionsBuilder = new ApplicationCommandOptionBuilder();
        var parameters = method.GetParameters();

        var strParam = parameters[0];
        var intParam = parameters[1];
        var numParam = parameters[2];
        var boolParam = parameters[3];
        var chanParam = parameters[4];
        var userParam = parameters[5];
        var errParam = parameters[6];

        Assert.Throws<InvalidOperationException>(() => optionsBuilder.BuildOption(errParam));
        
        var strInfo = optionsBuilder.BuildOption(strParam);
        optionsBuilder.BuildOption(intParam);
        optionsBuilder.BuildOption(numParam);
        optionsBuilder.BuildOption(boolParam);
        optionsBuilder.BuildOption(chanParam);
        optionsBuilder.BuildOption(userParam);
        
        Assert.True(strInfo.Name == "string-param" &&
                    strInfo.Description == "example" &&
                    strInfo.Choices != null &&
                    strInfo.Choices.Length > 0 &&
                    strInfo.Choices[0].Name == "a"); //String parameter validation
    }
    
    private class TestClassA
    {
        public void TestMethodA(
            [Option("string-param", "example")]
            [OptionChoice("a", "a")]
            [OptionChoice("a", "b")]
            string stringParam,
            [Option("int-param", "example")]
            int integerParam,
            [Option("num-param", "example")]
            double numberParam,
            [Option("bool-param", "example")]
            bool booleanParam,
            [Option("channel-param", "example")]
            IDiscordChannel channelParam,
            [Option("user-param", "example")]
            IDiscordUser userParam,
            ErrorClass errMethod)
        {
            
        }
    }

    private class ErrorClass
    {
        
    }
}