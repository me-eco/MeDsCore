# MeDsCore. Discord Bot Client
MeDsCore is an async Discord Bot API library. `me.ds` module uses this library to interact with Discord

# Quick start
1. Create a Service Worker project template
2. Add in your  `configuration.json` the following JSON property
```json
"DsToken": "<YOUR_BEAUTIFUL_TOKEN>"
```
3. Replace code in the `Program.cs` by this
```c#
using MeDsCore;
using MeDsCore.WebSocket.Gateway;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(x =>
    {
        x.ClearProviders();
        x.AddMeDsBotLogger();
        x.SetMinimumLevel(LogLevel.Trace);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<BotWorker>()            
            .AddSingleton<DiscordClient>()
            .AddSingleton(new DiscordClientConfiguration(context.Configuration["DsToken"])
            {
                Intents = GatewayIntents.GuildsIntent |
                    GatewayIntents.GuildPresencesIntent |
                    GatewayIntents.GuildMessagesIntent
            });
    })
    .Build();

await host.RunAsync();
```
4. Create a class named `BotWorker`
```c#
using System.Reflection;
using MeDsCore;

namespace MeDsBot.Workers;

public class BotWorker : BackgroundService
{
    private readonly DiscordClient _discordClient;

    public BotWorker(DiscordClient discordClient)
    {
        _discordClient = discordClient;       
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {   
        await _discordClient.StartAsync();
        await Task.Delay(-1, stoppingToken);
    }
}
```
**Result:**

![img.png](./brand/img.png)