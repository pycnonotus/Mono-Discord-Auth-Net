namespace Pycnonotus.MonoDiscordAuth.Options;

internal static class DiscordAuthOptionsFailures
{
    public const string ClientIdMissing = "Missing Discord:ClientId configuration value.";
    public const string ClientSecretMissing = "Missing Discord:ClientSecret configuration value.";
    public const string AdminUserIdMissing = "Missing Discord:AdminUserId configuration value.";
    public const string CallbackPathInvalid = "Discord:CallbackPath must be a rooted path.";

}