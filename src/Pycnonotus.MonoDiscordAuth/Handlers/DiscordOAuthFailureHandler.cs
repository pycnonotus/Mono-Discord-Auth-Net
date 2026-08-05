using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Pycnonotus.MonoDiscordAuth.Handlers;

internal static class DiscordOAuthFailureHandler
{
    public static async Task HandleAsync(RemoteFailureContext context)
    {
        await context.HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        context.HandleResponse();

        context.Response.Redirect("/auth/denied");
    }
}
