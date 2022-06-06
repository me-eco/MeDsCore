using MeDsCore.Rest.Discord;

namespace MeDsCore.Abstractions;

/// <summary>
/// Абстракция сервера Discord
/// </summary>
public interface IDiscordGuild
{
    public ulong Id { get; }
    public string Name { get; }
    /// <summary>
    /// Список всех участников сервера
    /// </summary>
    IAsyncEnumerable<DiscordGuildMember> GuildMembers { get; }
    /// <summary>
    /// Возвращает все каналы, которые есть на сервере
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<IDiscordChannel> GetChannelsAsync();
    /// <summary>
    /// Возвращает все <b>текстовые</b> каналы, которые есть на сервере
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<DiscordTextChannel> GetTextChannelsAsync();

    Task<DiscordGuildMember?> GetMemberAsync(ulong id);
}