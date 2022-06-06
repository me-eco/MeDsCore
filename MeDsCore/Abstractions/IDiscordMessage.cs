namespace MeDsCore.Abstractions;

public interface IDiscordMessage
{
    public ulong Id { get; }
    public string Content { get; }
    public ulong ChannelId { get; }
    
    Task DeleteAsync();
}