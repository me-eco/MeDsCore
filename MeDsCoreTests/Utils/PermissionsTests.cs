using MeDsCore.Utils;
using Xunit;

namespace MeDsCoreTests.Utils;

public class PermissionsTests
{
    [Fact]
    public void ComputePermissions()
    {
        var permissions = Permission.SendMessagesInThreads | Permission.AddReactions;

        var permsDeserialized = PermissionComputer.ComputePermissions((long)permissions);

        Permission perms = 0x0;
        
        foreach (var permission in permsDeserialized)
        {
            perms |= permission;
        }
        
        Assert.Equal(permissions, perms);
    }
}