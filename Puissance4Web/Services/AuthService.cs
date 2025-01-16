using System.Net.Http.Headers;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Définir le token JWT dans l'en-tête Authorization
    public void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Supprimer le token JWT de l'en-tête Authorization
    public void ClearToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}