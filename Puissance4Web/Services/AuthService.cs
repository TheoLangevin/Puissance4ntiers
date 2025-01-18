using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

public class AuthService
{
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;
    private bool _isInitialized = false;

    public AuthService(ILocalStorageService localStorage, NavigationManager navigation)
    {
        _localStorage = localStorage;
        _navigation = navigation;
    }

    public async Task<bool> IsTokenValid()
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("AuthService is not fully initialized. Wait until the component is rendered.");
        }

        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null || jwtToken.ValidTo < DateTime.UtcNow)
        {
            await Logout();
            return false;
        }

        return true;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _navigation.NavigateTo("/login", forceLoad: true);
    }

    public void MarkAsInitialized()
    {
        _isInitialized = true;
    }
}