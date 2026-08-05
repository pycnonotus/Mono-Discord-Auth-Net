using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pycnonotus.MonoDiscordAuth.DiscordUserIds;

public sealed class DiscordUserIdJsonConverter : JsonConverter<DiscordUserId>
{
    public override DiscordUserId Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetUInt64(out var numericValue) => numericValue,
            JsonTokenType.String => ReadStringValue(ref reader),
            _ => throw new JsonException("Discord user id must be a string or an unsigned integer.")
        };

        try
        {
            return new DiscordUserId(value);
        }
        catch (ArgumentException exception)
        {
            throw new JsonException(exception.Message, exception);
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        DiscordUserId value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString(CultureInfo.InvariantCulture));
    }

    private static ulong ReadStringValue(ref Utf8JsonReader reader)
    {
        var value = reader.GetString();

        if (!ulong.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var numericValue))
            throw new JsonException("Discord user id must be a valid unsigned integer.");

        return numericValue;
    }
}