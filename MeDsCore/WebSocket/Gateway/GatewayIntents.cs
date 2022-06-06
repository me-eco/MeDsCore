namespace MeDsCore.WebSocket.Gateway;

public struct GatewayIntents
{
    private const int Guilds = 1 << 0,
        GuildMembers = 1 << 1,
        GuildBans = 1 << 2,
        GuildEmojisAndStickers = 1 << 3,
        GuildIntegrations = 1 << 4,
        GuildWebhooks = 1 << 5,
        GuildInvites = 1 << 6,
        GuildVoiceStates = 1 << 7,
        GuildPresences = 1 << 8,
        GuildMessages = 1 << 9,
        GuildMessageReactions = 1 << 10,
        GuildMessageTyping = 1 << 11,
        DirectMessages = 1 << 12,
        DirectMessageReactions = 1 << 13,
        DirectMessageTyping = 1 << 14,
        MessageContent = 1 << 15,
        GuildSheduledEvents = 1 << 16;

    public static readonly GatewayIntents GuildsIntent = new GatewayIntents(Guilds),
        GuildMembersIntent = new GatewayIntents(GuildMembers),
        GuildBansIntent = new GatewayIntents(GuildBans),
        GuildEmojisAndStickersIntent = new GatewayIntents(GuildEmojisAndStickers),
        GuildIntegrationsIntent = new GatewayIntents(GuildIntegrations),
        GuildWebhooksIntent = new GatewayIntents(GuildWebhooks),
        GuildInvitesIntent = new GatewayIntents(GuildInvites),
        GuildVoiceStatesIntent = new GatewayIntents(GuildVoiceStates),
        GuildPresencesIntent = new GatewayIntents(GuildPresences),
        GuildMessagesIntent = new GatewayIntents(GuildMessages),
        GuildMessageReactionsIntent = new GatewayIntents(GuildMessageReactions),
        GuildMessageTypingIntent = new GatewayIntents(GuildMessageTyping),
        DirectMessagesIntent = new GatewayIntents(DirectMessages),
        DirectMessageReactionsIntent = new GatewayIntents(DirectMessageReactions),
        DirectMessageTypingIntent = new GatewayIntents(DirectMessageTyping),
        MessageContentIntent = new GatewayIntents(MessageContent),
        GuildSheduledEventsIntent = new GatewayIntents(GuildSheduledEvents),
        None = new GatewayIntents(0),
        All = new GatewayIntents(0b1111111111111111);

    public int Intents { get; }
    
    private GatewayIntents(int intents)
    {
        Intents = intents;
    }

    public static GatewayIntents operator |(GatewayIntents a, GatewayIntents b)
    {
        return new GatewayIntents(a.Intents + b.Intents);
    }
}