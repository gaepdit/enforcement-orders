using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Enfo.Domain.Utils;

[DebuggerStepThrough]
public static class Guard
{
    public static T NotNull<T>(T value, string parameterName) =>
        value ?? throw new ArgumentNullException(parameterName);

    public static string NotNullOrWhiteSpace(string value, string parameterName)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        if (value.IsNullOrWhiteSpaceString())
            throw new ArgumentException($"{parameterName} cannot be null, empty, or white space.", parameterName);

        return value;
    }

    public static int NotNegative(int value, string parameterName) =>
        value >= 0
            ? value
            : throw new ArgumentException($"{parameterName} cannot be negative.", parameterName);

    public static int Positive(int value, string parameterName) =>
        value > 0
            ? value
            : throw new ArgumentException($"{parameterName} must be positive (greater than zero).", parameterName);

    public static string RegexMatch(string value, string parameterName, string pattern)
    {
        if (value is null) return null;

        return Regex.IsMatch(value, NotNull(pattern, nameof(pattern)))
            ? value
            : throw new ArgumentException($"Value ({value}) is not valid.", parameterName);
    }
}
