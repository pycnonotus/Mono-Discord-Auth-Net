namespace Pycnonotus.MonoDiscordAuth.Options;

internal static class AuthPathValidator
{
    public static bool IsRootedPath(string? path)
    {
        return !string.IsNullOrWhiteSpace(path)
               && path.StartsWith("/", StringComparison.Ordinal);
    }
}