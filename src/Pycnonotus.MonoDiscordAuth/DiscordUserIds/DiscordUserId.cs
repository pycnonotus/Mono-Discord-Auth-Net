using System.Text.Json.Serialization;

namespace Pycnonotus.MonoDiscordAuth.DiscordUserIds;

[JsonConverter(typeof(DiscordUserIdJsonConverter))]
public readonly record struct DiscordUserId
{
    public ulong Value { get; }

    public DiscordUserId(ulong value)
    {
        if (value == 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Discord user id cannot be zero.");

        Value = value;
    }

    public static DiscordUserId FromString(string value)
    {
        return new DiscordUserId(ulong.Parse(value));
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}