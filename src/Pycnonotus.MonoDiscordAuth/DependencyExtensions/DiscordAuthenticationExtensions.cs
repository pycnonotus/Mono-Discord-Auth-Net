using Microsoft.AspNetCore.Builder;

namespace Pycnonotus.MonoDiscordAuth.DependencyExtensions;

public static class DiscordAuthenticationExtensions
{
    public static WebApplicationBuilder AddDiscordAuthentication(this WebApplicationBuilder builder)
    {
        // builder.AddDiscordAuthenticationOptions();
        // builder.AddCookieAndDiscordOAuthAuthentication();

        return builder;
    }
}