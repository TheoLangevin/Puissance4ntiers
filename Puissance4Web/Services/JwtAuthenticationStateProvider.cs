using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
    private bool _isPrerendering = true;

    public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_isPrerendering)
        {
            // Pendant le prérendu, retourner un utilisateur non authentifié
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(token) || !IsTokenValid(token))
        {
            // Si le token est invalide ou absent, retourner un utilisateur non authentifié
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = ExtractClaimsFromToken(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    private bool IsTokenValid(string token)
    {
        try
        {
            var jwtToken = _tokenHandler.ReadToken(token) as JwtSecurityToken;
            return jwtToken != null && jwtToken.ValidTo > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }

    private IEnumerable<Claim> ExtractClaimsFromToken(string token)
    {
        var jwtToken = _tokenHandler.ReadToken(token) as JwtSecurityToken;
        return jwtToken?.Claims ?? Enumerable.Empty<Claim>();
    }

    public async Task Login(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void MarkAsInteractive()
    {
        _isPrerendering = false;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}