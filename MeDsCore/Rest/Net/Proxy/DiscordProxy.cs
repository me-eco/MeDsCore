using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using MeDsCore.Abstractions;
using Microsoft.Extensions.Logging;

namespace MeDsCore.Rest.Net.Proxy;

internal class DiscordProxy : IDiscordProxy
{
    private readonly string _baseUrl;
    private readonly ILogger _logger;
    private readonly HttpClient _client;
    
    public DiscordProxy(string baseUrl, ILogger logger)
    {
        if (!Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            throw new InvalidOperationException("Invalid base url");
        _baseUrl = baseUrl;
        _logger = logger;

        _client = new HttpClient();
    }
    
    public async Task<long> PingAsync()
    {
        try
        {
            var ping = new Ping();
            var targetUri = new Uri(_baseUrl);
            var options = new PingOptions()
            {
                DontFragment = true
            };
            string pingContent = "botbotyabotbotyabotbotyabotbotyabotbotya";

            byte[] buffer = Encoding.ASCII.GetBytes(pingContent);
            
            
            var reply = await ping.SendPingAsync(targetUri.Host, 1024, buffer, options);
            var roundtripTime = reply.RoundtripTime;
            
            
            _logger.LogTrace($"Discord API Ping {roundtripTime}");
            
            return roundtripTime;
        }
        catch (Exception)
        {
            _logger.LogError("Failed to ping Discord API");
            return -1;
        }
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
    {
        message.RequestUri = new Uri(_baseUrl + message.RequestUri!.OriginalString);
        return _client.SendAsync(message);
    }
}