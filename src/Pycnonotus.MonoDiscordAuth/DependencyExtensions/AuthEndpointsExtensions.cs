using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Pycnonotus.MonoDiscordAuth.Constants;

namespace Pycnonotus.MonoDiscordAuth.DependencyExtensions;

public static class AuthEndpointsExtensions
{
    private const string DefaultReturnUrl = "/";

    public static IEndpointRouteBuilder MapDiscordAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var authGroup = endpoints.MapGroup(DiscordAuthPaths.Group);

        authGroup.MapGet(DiscordAuthPaths.Login, Login)
            .AllowAnonymous();
        authGroup.MapPost(DiscordAuthPaths.Logout, async (httpContext) => await Logout(httpContext))
            .RequireAuthorization();
        authGroup.MapGet(DiscordAuthPaths.AccessDenied, Denied)
            .AllowAnonymous();
        authGroup.MapGet(DiscordAuthPaths.NotRegistered, NotRegistered)
            .AllowAnonymous();

        return endpoints;
    }


    private static IResult Login(string? returnUrl)
    {
        return Results.Challenge(
            new AuthenticationProperties
            {
                RedirectUri = GetSafeRedirectUri(returnUrl)
                // RedirectUri = "http://localhost:5173/dashboard/"
            },
            [DiscordAuthConstants.AuthenticationScheme]);
    }

    private static string GetSafeRedirectUri(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
            return DefaultReturnUrl;

        return IsLocalPath(returnUrl)
            ? returnUrl
            : DefaultReturnUrl;
    }

    private static bool IsLocalPath(string returnUrl)
    {
        if (returnUrl[0] != '/')
            return false;

        return returnUrl.Length == 1 || returnUrl[1] is not ('/' or '\\');
    }

    private static async Task<IResult> Logout(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Results.NoContent();
    }

    private static IResult Denied()
    {
        return Results.Forbid();
    }

    private static IResult NotRegistered()
    {
        return Results.Text(
            DiscordAuthConstants.UnregisteredDiscordUser,
            statusCode: StatusCodes.Status403Forbidden);
    }
}