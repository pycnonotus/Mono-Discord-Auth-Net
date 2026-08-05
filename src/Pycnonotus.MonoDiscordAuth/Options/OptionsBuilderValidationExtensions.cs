using Microsoft.Extensions.Options;

namespace Pycnonotus.MonoDiscordAuth.Options;

internal static class OptionsBuilderValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateRequired<TOptions>(
        this OptionsBuilder<TOptions> builder,
        Func<TOptions, string?> valueSelector,
        string failureMessage)
        where TOptions : class
    {
        return builder.Validate(
            options => !string.IsNullOrWhiteSpace(valueSelector(options)),
            failureMessage);
    }
}