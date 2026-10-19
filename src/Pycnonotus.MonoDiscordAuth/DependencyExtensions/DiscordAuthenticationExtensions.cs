using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pycnonotus.MonoDiscordAuth.Constants;
using Pycnonotus.MonoDiscordAuth.Handlers;
using Pycnonotus.MonoDiscordAuth.Options;

namespace Pycnonotus.MonoDiscordAuth.DependencyExtensions;

public static class DiscordAuthenticationExtensions
{
    public static WebApplicationBuilder AddDiscordAuthentication(this WebApplicationBuilder builder)
    {
        builder.AddDiscordAuthenticationOptions();
        builder.AddCookieAndDiscordOAuthAuthentication();
        return builder;
    }
    
    private static void AddDiscordAuthenticationOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<DiscordAuthOptions>()
            .BindConfiguration(DiscordAuthOptions.SectionName)
            .ValidateRequired(options => options.ClientId, DiscordAuthOptionsFailures.ClientIdMissing)
            .ValidateRequired(options => options.ClientSecret, DiscordAuthOptionsFailures.ClientSecretMissing)
            .Validate(
                options => options.AdminUserIds is { Length: > 0 }
                           && options.AdminUserIds.All(id => !string.IsNullOrWhiteSpace(id)),
                DiscordAuthOptionsFailures.AdminUserIdsMissing)
            .Validate(
                options => AuthPathValidator.IsRootedPath(options.CallbackPath),
                DiscordAuthOptionsFailures.CallbackPathInvalid)
            .ValidateOnStart();
    }

    private static void AddCookieAndDiscordOAuthAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = DiscordAuthConstants.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = DiscordAuthConstants.SessionCookieName;
            })
            .AddOAuth<OAuthOptions, DiscordOAuthHandler>(
                DiscordAuthConstants.AuthenticationScheme,
                options => builder.ConfigureDiscordOAuthScheme(options));
    }
    
    private static void ConfigureDiscordOAuthScheme(
        this WebApplicationBuilder builder,
        OAuthOptions options)    {

        var discordOptions = builder.Configuration
            .GetRequiredSection(DiscordAuthOptions.SectionName)
            .Get<DiscordAuthOptions>()!;
        
        options.ClientId = discordOptions.ClientId;
        options.ClientSecret = discordOptions.ClientSecret;
        options.CallbackPath = discordOptions.CallbackPath;
        options.CorrelationCookie.Name = DiscordAuthConstants.CorrelationCookieName;

        options.AuthorizationEndpoint = DiscordAuthEndpoints.Authorization;
        options.TokenEndpoint = DiscordAuthEndpoints.Token;
        options.UserInformationEndpoint = DiscordAuthEndpoints.UserInformation;

        options.Scope.Add(DiscordAuthConstants.IdentifyScope);

        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, DiscordUserJsonFields.Id);

        options.ClaimActions.MapJsonKey(ClaimTypes.Name, DiscordUserJsonFields.Username);

        options.Events.OnRemoteFailure = DiscordOAuthFailureHandler.HandleAsync;
    }
}
