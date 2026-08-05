using Pycnonotus.MonoDiscordAuth.DependencyExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDiscordAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();

app.MapDiscordAuthEndpoints();

app.MapGet("/undaunted", () => "you can see undaunted data");
app.MapGet("/aunted", () => "you can see aunted data").RequireAuthorization();

app.Run();