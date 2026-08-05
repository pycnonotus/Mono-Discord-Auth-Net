namespace Pycnonotus.MonoDiscordAuth.Constants;

internal static class DiscordAuthConstants
{
    public const string AuthenticationScheme = "Discord";
    public const string IdentifyScope = "identify";
    public const string BearerTokenType = "Bearer";
    public const string SessionCookieName = "giveaway.session";
    public const string CorrelationCookieName = "giveaway.oauth.";

    public const string UserInformationRequestFailed = "Failed to load Discord user profile.";
    public const string UnregisteredDiscordUser = "Discord user is not registered.";
}

