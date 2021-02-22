using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enfo.Utils;

namespace Enfo.Repository.Utils
{
    [DebuggerStepThrough]
    public static class Guard
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            return value ?? throw new ArgumentNullException(parameterName);
        }

        public static string NotNullOrEmpty(string value, string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.IsNullOrEmptyString())
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
            }

            return value;
        }

        public static string NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.IsNullOrWhiteSpaceString())
            {
                throw new ArgumentException($"{parameterName} cannot be null, empty, or white space.", parameterName);
            }

            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value is null || value.Count == 0)
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
            }

            return value;
        }

        public static int NotNegative(int value, string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentException($"{parameterName} cannot be negative.", parameterName);
            }

            return value;
        }

        public static int Positive(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{parameterName} must be positive (greater than zero).", parameterName);
            }

            return value;
        }
    }
}