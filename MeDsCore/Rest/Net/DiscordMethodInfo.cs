namespace MeDsCore.Rest.Net;

/// <summary>
/// Contains metadata about the REST API method
/// </summary>
internal class DiscordMethodInfo
{
    private readonly string _route;
    private readonly HttpMethod _httpMethod;

    public DiscordMethodInfo(string route, HttpMethod httpMethod)
    {
        _route = route;
        _httpMethod = httpMethod;
    }

    /// <summary>
    /// Creates new scaffold <see cref="HttpRequestMessage"/> 
    /// </summary>
    public HttpRequestMessage ConfigureScaffoldHttpRequest()
    {
        return new HttpRequestMessage()
        {
            Method = _httpMethod,
            RequestUri = new Uri(_route, UriKind.Relative)
        };
    }

    public static DiscordMethodInfo Get(string route) => new(route, HttpMethod.Get);
    public static DiscordMethodInfo Post(string route) => new(route, HttpMethod.Post);
    public static DiscordMethodInfo Delete(string route) => new(route, HttpMethod.Delete);
    public static DiscordMethodInfo Put(string route) => new(route, HttpMethod.Put);
}