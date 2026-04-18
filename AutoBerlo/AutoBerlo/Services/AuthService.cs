using System.IdentityModel.Tokens.Jwt;

namespace AutoBerlo.Services;

public class AuthService
{
    private const string TokenKey = "auth_token";
    private string? _cachedToken;

    public string? Token
    {
        get => _cachedToken ??= Preferences.Get(TokenKey, null);
        private set
        {
            _cachedToken = value;
            if (value != null)
                Preferences.Set(TokenKey, value);
            else
                Preferences.Remove(TokenKey);
        }
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Token) && !IsTokenExpired();

    public void SaveToken(string token) => Token = token;

    public void Logout()
    {
        Token = null;
        _cachedToken = null;
    }

    public void SetAuthHeader(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
    }

    private bool IsTokenExpired()
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(Token);
            return jwt.ValidTo < DateTime.UtcNow;
        }
        catch { return true; }
    }
}