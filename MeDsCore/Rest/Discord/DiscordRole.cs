using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Utils;

namespace MeDsCore.Rest.Discord;

public class DiscordRole : IDiscordRole
{
    public DiscordRole(RoleEntity entity)
    {
        Id = ulong.Parse(entity.Id);
        Name = entity.Name;
        Position = entity.Position;

        var permissions = long.Parse(entity.Permissions);
        Permissions = PermissionComputer.ComputePermissions(permissions);
    }
    
    public ulong Id { get; }
    public string Name { get; }
    public int Position { get; }
    public IEnumerable<Permission> Permissions { get; }
}