using Microsoft.AspNetCore.Routing;

namespace Pycnonotus.MonoDiscordAuth.DependencyExtensions;

public static class AuthEndpointsExtensions
{
    public static IEndpointRouteBuilder MapDiscordAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // var authGroup = endpoints.MapGroup(DiscordAuthPaths.Group);
        //
        // authGroup.MapGet(DiscordAuthPaths.Login, Login)
        //     .AllowAnonymous();
        // authGroup.MapPost(DiscordAuthPaths.Logout, async (httpContext) => await Logout(httpContext))
        //     .RequireAuthorization();
        // authGroup.MapGet(DiscordAuthPaths.AccessDenied, Denied)
        //     .AllowAnonymous();
        // authGroup.MapGet(DiscordAuthPaths.NotRegistered, NotRegistered)
        //     .AllowAnonymous();

        return endpoints;
    }
}