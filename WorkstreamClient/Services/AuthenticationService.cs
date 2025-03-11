using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WorkstreamClient.DTO;
using System.IdentityModel.Tokens.Jwt;


namespace WorkstreamClient.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        public AuthenticationService(HttpClient httpClient, NavigationManager navigationManager, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                var token = result?.Token;

                if (token != null)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    _navigationManager.NavigateTo("/dashboard");
                    return token;
                }
            }

            return null;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken"); // Ensure token is removed before proceeding
            _httpClient.DefaultRequestHeaders.Authorization = null; // Remove token from HTTP client
            _navigationManager.NavigateTo("/login", forceLoad: true); // Force reload to clear state
        }


        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            return !string.IsNullOrEmpty(token);
        }

        // Optional: Token Expiry check
        public bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var expiry = jsonToken?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (expiry == null) return true;
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry));
            return expirationDate < DateTimeOffset.UtcNow;
        }
    }
}
