using MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core;
using MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;
using Xunit;

namespace MeDsCoreTests.Interactions.ApplicationCommandsModules;

public class CommandBuildingTests
{
    [Fact]
    public void BuildCommandTest_Must_Build()
    {
        var method = typeof(TestClass).GetMethod("TestMethodA")!;
        var commandBuilder = new ApplicationCommandBuilder(false, 0);
        var command = commandBuilder.BuildInfo(method);
        
        Assert.Equal("test", command.Name);
        Assert.IsType<GlobalChatInputCommandInfo>(command);

        var slash = (command as GlobalChatInputCommandInfo)!;
        
        Assert.Single(slash.Options);
    }
    
    [Fact]
    public void BuildCommandTest_Must_Build_Without_Params()
    {
        var method = typeof(TestClass).GetMethod("TestMethodB")!;
        var commandBuilder = new ApplicationCommandBuilder(false, 0);
        var command = commandBuilder.BuildInfo(method);
        
        Assert.Equal("test", command.Name);
        Assert.IsType<GlobalChatInputCommandInfo>(command);

        var slash = (command as GlobalChatInputCommandInfo)!;
        
        Assert.Empty(slash.Options);
    }

    [Fact]
    public void BuildCommandTest_Private()
    {
        var method = typeof(TestClass).GetMethod("TestMethodC")!;
        var commandBuilder = new ApplicationCommandBuilder(false, 0);
        var command = commandBuilder.BuildInfo(method);
        
        Assert.IsType<PrivateChatInputCommandInfo>(command);
    }

    private class TestClass
    {
        [SlashCommand("test", "d")]
        public void TestMethodA([Option("a", "a")] string a)
        {
            
        }

        [SlashCommand("test", "d")]
        public void TestMethodB()
        {
        }

        [TargetGuild(0)]
        [SlashCommand("test", "a")]
        public void TestMethodC()
        {
            
        }
    }
}