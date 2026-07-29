using Pycnonotus.MonoDiscordAuth.DependencyExtensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.AddDiscordAuthentication();
builder.Services.AddOpenApi();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();

app.MapDiscordAuthEndpoints();

app.MapGet("/undaunted", () => "you can see undaunted data");
app.MapGet("/aunted", () => "you can see undaunted data").RequireAuthorization();

app.Run();