using MeDsCore.Rest.Discord;

namespace MeDsCore.Abstractions;

public interface IDiscordUser
{
    public ulong Id { get; }
    public string Username { get; }
    public bool IsBot { get; }

    public Task<DiscordTextChannel> CreateDmChannelAsync();
}