using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace API.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if the API key header is present
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // Validate the API key
        if (!Options.ApiKey.Equals(providedApiKey, StringComparison.Ordinal))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }

        // Create claims for the authenticated user
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "ApiKeyUser"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        Response.Headers.Append("WWW-Authenticate", $"ApiKey realm=\"{Options.Realm}\"");
        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    }
}

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "ApiKey";
    public string Scheme => DefaultScheme;
    public string ApiKey { get; set; } = string.Empty;
    public string Realm { get; set; } = "Dead by Daylight API";
}

public static class ApiKeyAuthenticationExtensions
{
    public static AuthenticationBuilder AddApiKeyAuthentication(
        this IServiceCollection services,
        Action<ApiKeyAuthenticationOptions> configureOptions)
    {
        return services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationOptions.DefaultScheme,
                configureOptions);
    }
}
