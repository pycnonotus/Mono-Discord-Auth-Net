namespace Pycnonotus.MonoDiscordAuth.Options;

internal static class DiscordAuthOptionsFailures
{
    public const string ClientIdMissing = "Missing Discord:ClientId configuration value.";
    public const string ClientSecretMissing = "Missing Discord:ClientSecret configuration value.";
    public const string AdminUserIdsMissing = "Discord:AdminUserIds must contain at least one non-empty user ID.";
    public const string CallbackPathInvalid = "Discord:CallbackPath must be a rooted path.";

}
