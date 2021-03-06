using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Enfo.Domain.Utils
{
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

        public static int NotNegative(int value, string parameterName)
        {
            if (value < 0)
                throw new ArgumentException($"{parameterName} cannot be negative.", parameterName);

            return value;
        }

        public static int Positive(int value, string parameterName)
        {
            if (value <= 0)
                throw new ArgumentException($"{parameterName} must be positive (greater than zero).", parameterName);

            return value;
        }

        public static void RegexMatch(string value, string parameterName, string pattern)
        {
            NotNull(pattern, nameof(pattern));

            if (value == null) return;

            if (!Regex.IsMatch(value, pattern))
                throw new ArgumentException($"Value ({value}) is not valid.", parameterName);
        }
    }
}
