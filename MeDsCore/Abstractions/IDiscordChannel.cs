using MeDsCore.Rest.Entities;
using MeDsCore.Base;

namespace MeDsCore.Abstractions;

public interface IDiscordChannel
{
    public ulong Id { get; }
    public string Name { get; }
    public ChannelType ChannelType { get; }
}