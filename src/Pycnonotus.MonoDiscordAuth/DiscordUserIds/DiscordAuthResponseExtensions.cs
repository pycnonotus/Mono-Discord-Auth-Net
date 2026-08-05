using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Pycnonotus.MonoDiscordAuth.Constants;

namespace Pycnonotus.MonoDiscordAuth.DiscordUserIds;

internal static class DiscordAuthResponseExtensions
{
    internal static bool TryGetDiscordUserId(this JsonDocument payload,[NotNullWhen(true)] out DiscordUserId? id)
    {
        id = null;

        if (!payload.RootElement.TryGetProperty(DiscordUserJsonFields.Id, out var idProperty))
            return false;

        if (idProperty.ValueKind == JsonValueKind.String)
        {
            var idString = idProperty.GetString();
            if(string.IsNullOrWhiteSpace(idString))
                return false;
            
            id = DiscordUserId.FromString(idString);
            return true;
        }

        if (idProperty.ValueKind == JsonValueKind.Number &&
            idProperty.TryGetUInt64(out var numericId))
        {
            
            id = new DiscordUserId(numericId);
            return true;
        }

        return false;
    }
}