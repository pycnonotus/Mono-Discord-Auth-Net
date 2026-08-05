using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pycnonotus.MonoDiscordAuth.Constants;
using Pycnonotus.MonoDiscordAuth.DiscordUserIds;
using Pycnonotus.MonoDiscordAuth.Options;

namespace Pycnonotus.MonoDiscordAuth.Handlers;

internal sealed class DiscordOAuthHandler(
    IOptionsMonitor<OAuthOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<DiscordAuthOptions> discordOptions)
    : OAuthHandler<OAuthOptions>(options, logger, encoder)
{
    private readonly DiscordAuthOptions _discordOptions = discordOptions.Value;

    protected override async Task<AuthenticationTicket> CreateTicketAsync(
        ClaimsIdentity identity,
        AuthenticationProperties properties,
        OAuthTokenResponse tokens)
    {
        using var request = CreateUserInformationRequest(tokens);


        using var response = await Backchannel.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            Context.RequestAborted);

        if (!response.IsSuccessStatusCode)
        {
            throw new AuthenticationFailureException(
                DiscordAuthConstants.UserInformationRequestFailed);
        }

        await using var stream = await response.Content.ReadAsStreamAsync(
            Context.RequestAborted);

        using var payload = await JsonDocument.ParseAsync(
            stream,
            cancellationToken: Context.RequestAborted);

        if (await IsUserInPool(payload) != IsUserInPoolResult.InPool)
            throw new AuthenticationException(DiscordAuthConstants.UnregisteredDiscordUser);

        var principal = new ClaimsPrincipal(identity);

        var ticketContext = new OAuthCreatingTicketContext(
            principal,
            properties,
            Context,
            Scheme,
            Options,
            Backchannel,
            tokens,
            payload.RootElement);

        ticketContext.RunClaimActions(payload.RootElement);

        await Events.CreatingTicket(ticketContext);

        return new AuthenticationTicket(
            ticketContext.Principal!,
            ticketContext.Properties,
            Scheme.Name);
    }

    private async Task<IsUserInPoolResult> IsUserInPool(JsonDocument payload)
    {
        if (!payload.TryGetDiscordUserId(out var id))
            return IsUserInPoolResult.FailedToExtractId;

        if (IsAdmin(id.Value.ToString()))
            return IsUserInPoolResult.InPool;


        var userPool = Context.RequestServices.GetService<IUserPool>();
        if (userPool == null)
        {
            return IsUserInPoolResult.NotInPool;
        }

        return (await userPool.IsUserInPoolAsync(id.Value)) 
            ? IsUserInPoolResult.InPool 
            : IsUserInPoolResult.NotInPool;
    }


    private enum IsUserInPoolResult
    {
        FailedToExtractId,
        NotInPool,
        InPool
    }

    private bool IsAdmin(in string id)
    {
        return string.Equals(id, _discordOptions.AdminUserId, StringComparison.OrdinalIgnoreCase);
    }

    private HttpRequestMessage CreateUserInformationRequest(OAuthTokenResponse tokens)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            Options.UserInformationEndpoint);

        request.Headers.Authorization = new AuthenticationHeaderValue(
            DiscordAuthConstants.BearerTokenType,
            tokens.AccessToken);

        return request;
    }
}