using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text.Json;
using WorkstreamClient.Services;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationService _authenticationService;

    public AuthStateProvider(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        // Check if the user is authenticated
        if (await _authenticationService.IsAuthenticatedAsync())
        {
            var token = await _authenticationService.GetTokenAsync();
            if (token != null)
            {
                // Parse claims from the JWT token
                user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "Bearer"));
            }
        }

        return new AuthenticationState(user);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = WebEncoders.Base64UrlDecode(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "Bearer"));
        var authenticationState = new AuthenticationState(authenticatedUser);
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authenticationState = new AuthenticationState(anonymousUser);
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    }
}
