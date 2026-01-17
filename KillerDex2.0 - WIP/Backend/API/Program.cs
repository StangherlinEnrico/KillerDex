using Application;
using Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Dead by Daylight API";
        document.Info.Version = "v1";
        document.Info.Description = "Unofficial API for Dead by Daylight game data. This project is not affiliated with or endorsed by Behaviour Interactive.";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "GitHub Repository",
            Url = new Uri("https://github.com/yourusername/dbd-api")
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
app.UseAuthorization();
app.MapControllers();

app.Run();
