using MeDsCore.Utils;

namespace MeDsCore.Abstractions;

public interface IDiscordRole
{
    ulong Id { get; }
    string Name { get; }   
    int Position { get; }
    IEnumerable<Permission> Permissions { get; }
}