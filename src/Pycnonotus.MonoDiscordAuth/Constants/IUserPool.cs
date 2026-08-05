using Pycnonotus.MonoDiscordAuth.DiscordUserIds;

namespace Pycnonotus.MonoDiscordAuth.Constants;

public interface IUserPool
{
    Task<bool> IsUserInPoolAsync(DiscordUserId discordUserId);
}