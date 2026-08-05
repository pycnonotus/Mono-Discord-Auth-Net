namespace Pycnonotus.MonoDiscordAuth.Options;

sealed class DiscordAuthOptions
{
    public const string SectionName = "Discord";

    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string AdminUserId { get; init; } = string.Empty;
    public string CallbackPath { get; init; } = "/signin-discord";
    public string[] AllowedReturnOrigins { get; init; } = [];
    public bool IsAddAuthEndpoint { get; init; } = false;
    
}