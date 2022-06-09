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

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
    {
        message.RequestUri = new Uri(_baseUrl + message.RequestUri!.OriginalString);
        return _client.SendAsync(message);
    }
}