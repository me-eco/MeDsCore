using MeDsCore.Base;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.Base;

public class InteractionApplicationCommandResponse
{
    public InteractionApplicationCommandResponse(InteractionApplicationCommandResponseEntity entity)
    {
        Id = ulong.Parse(entity.Id);
        Name = entity.Name;
        Type = entity.Type;
        GuildId = ulong.Parse(entity.GuildId);

        #region OPTIONS_IMPL

        var options = new InteractionResponseDataOption[entity.Options?.Length ?? 0];
        
        for (var i = 0; i < options.Length; i++)
        {
            options[i] = new InteractionResponseDataOption(entity.Options![i]);
        }

        Options = options;

        #endregion

        #region RESOLVED_IMPL

        if(entity.ResolvedData == null) return;

        IReadOnlyDictionary<ulong, UserEntity>? users = null;
        IReadOnlyDictionary<ulong, ChannelEntity>? channels = null;
        IReadOnlyDictionary<ulong, MessageEntity>? messages = null;
        IReadOnlyDictionary<ulong, RoleEntity>? roles = null;

        if (entity.ResolvedData.Users != null)
        {
            Dictionary<ulong, UserEntity> userEntities = new();

            foreach (var (id, user) in entity.ResolvedData.Users)
            {
                userEntities.Add(ulong.Parse(id), user);
            }

            users = userEntities;
        }

        if (entity.ResolvedData.Channels != null)
        {
            Dictionary<ulong, ChannelEntity> channelEntities = new();

            foreach (var (id, channel) in entity.ResolvedData.Channels)
            {
                channelEntities.Add(ulong.Parse(id), channel);
            }

            channels = channelEntities;
        }

        if (entity.ResolvedData.Messages != null)
        {
            Dictionary<ulong, MessageEntity> messageEntities = new();

            foreach (var (id, message) in entity.ResolvedData.Messages)
            {
                messageEntities.Add(ulong.Parse(id), message);
            }

            messages = messageEntities;
        }

        if (entity.ResolvedData.Roles != null)
        {
            Dictionary<ulong, RoleEntity> roleEntities = new();

            foreach (var (id, message) in entity.ResolvedData.Roles)
            {
                roleEntities.Add(ulong.Parse(id), message);
            }

            roles = roleEntities;
        }
        
        ResolvedData = new ResolvedData(users, channels, messages, roles);

        #endregion
    }
    
    public ulong Id { get; }
    public string Name { get; }
    public ApplicationCommandType Type { get; }
    public ulong GuildId { get; }
    public InteractionResponseDataOption[] Options { get; }
    public ResolvedData? ResolvedData { get; }
}

public class ResolvedData
{
    public ResolvedData(IReadOnlyDictionary<ulong, UserEntity>? users,
        IReadOnlyDictionary<ulong, ChannelEntity>? channels, 
        IReadOnlyDictionary<ulong, MessageEntity>? messages,
        IReadOnlyDictionary<ulong, RoleEntity>? roles)
    {
        Users = users;
        Channels = channels;
        Messages = messages;
        Roles = roles;
    }

    public IReadOnlyDictionary<ulong, UserEntity>? Users { get; }
    public IReadOnlyDictionary<ulong, ChannelEntity>? Channels { get; }
    public IReadOnlyDictionary<ulong, MessageEntity>? Messages { get; }
    public IReadOnlyDictionary<ulong, RoleEntity>? Roles { get; }
}