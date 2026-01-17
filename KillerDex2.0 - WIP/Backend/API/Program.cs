using API.Admin.Auth;
using API.Admin.Components;
using API.Authentication;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Blazor Server for Admin Panel
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AdminAuthenticationService>();

// Add authentication (Cookie for Admin, API Key for REST API)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/admin/login";
    options.LogoutPath = "/admin/logout";
    options.AccessDeniedPath = "/admin/login";
    options.Cookie.Name = "KillerDex.Admin";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
})
.AddApiKeyAuthentication(options =>
{
    options.ApiKey = builder.Configuration["Authentication:ApiKey"]
        ?? throw new InvalidOperationException("API Key not configured");
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Dead by Daylight API";
        document.Info.Version = "v1";
        document.Info.Description = "Unofficial API for Dead by Daylight game data. This project is not affiliated with or endorsed by Behaviour Interactive.\n\n**Authentication:** Write operations (POST, PUT, DELETE) require an API Key in the `X-Api-Key` header.";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "GitHub Repository",
            Url = new Uri("https://github.com/yourusername/dbd-api")
        };

        // Add API Key security scheme
        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSecurityScheme>();
        document.Components.SecuritySchemes["ApiKey"] = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Name = "X-Api-Key",
            Description = "API Key required for write operations"
        };

        return Task.CompletedTask;
    });
});

// CORS for public API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Dead by Daylight API");
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();

// Map Blazor Admin Panel
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Admin login endpoint (form POST)
app.MapPost("/admin/auth/login", async (HttpContext context, AdminAuthenticationService authService) =>
{
    var form = await context.Request.ReadFormAsync();
    var username = form["username"].ToString();
    var password = form["password"].ToString();
    var returnUrl = form["returnUrl"].ToString();

    if (string.IsNullOrEmpty(returnUrl))
        returnUrl = "/admin";

    var success = await authService.LoginAsync(username, password);
    if (success)
    {
        return Results.Redirect(returnUrl);
    }
    return Results.Redirect($"/admin/login?error=1&returnUrl={Uri.EscapeDataString(returnUrl)}");
}).DisableAntiforgery().AllowAnonymous();

// Admin logout endpoint
app.MapGet("/admin/logout", async (HttpContext context, AdminAuthenticationService authService) =>
{
    await authService.LogoutAsync();
    return Results.Redirect("/admin/login");
}).AllowAnonymous();

app.Run();
