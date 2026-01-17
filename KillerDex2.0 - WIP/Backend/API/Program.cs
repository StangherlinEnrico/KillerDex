using API.Authentication;
using Application;
using Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add API Key authentication
builder.Services.AddApiKeyAuthentication(options =>
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
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
