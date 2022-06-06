namespace MeDsCore;

/// <summary>
/// Provides Discord Authorization token
/// </summary>
public abstract class AuthorizationProvider
{
    public AuthorizationProvider(bool alwaysUseCachedValue)
    {
        _alwaysUseCachedValue = alwaysUseCachedValue;
    }
    
    protected abstract string ProvideToken();

    public string GetToken()
    {
        if (_alwaysUseCachedValue)
        {
            if (_cachedValue != null) return _cachedValue;
                
            var token = ProvideToken();
            _cachedValue = token;
            return token;
        }

        return ProvideToken();
    }

    private string? _cachedValue;
    private readonly bool _alwaysUseCachedValue;
    
    public void CreateAuth(HttpRequestMessage requestMessage)
    {
        var token = GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            requestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bot {token}");    
        }
    }

    public static AuthorizationProvider FromStaticToken(string token)
    {
        return new DefaultAuthorizationProvider(token);
    }
    
    private class DefaultAuthorizationProvider : AuthorizationProvider
    {
        private readonly string _token;

        public DefaultAuthorizationProvider(string token) : base(false)
        {
            _token = token;
        }

        protected override string ProvideToken()
        {
            return _token;
        }
    }
}