using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Enfo.Repository.Utils
{
    [DebuggerStepThrough]
    public static class Guard
    {
        public static T NotNull<T>(T value, string parameterName) =>
            value ?? throw new ArgumentNullException(parameterName);

        public static string NotNullOrEmpty(string value, string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException($"{parameterName} can not be null or empty.", parameterName);
            }

            return value;
        }

        public static string NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"{parameterName} can not be null, empty, or white space.", parameterName);
            }

            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException($"{parameterName} can not be null or empty.", parameterName);
            }

            return value;
        }

        public static int NotNegative(int value, string parameterName) =>
            value >= 0 ? value :
                throw new ArgumentException($"{parameterName} can not be negative.", parameterName);

        public static int Positive(int value, string parameterName) =>
            value > 0 ? value :
                throw new ArgumentException($"{parameterName} must be positive (greater than zero).", parameterName);

        //public static int AtLeast(int value, int atLeast, string parameterName) =>
        //    value >= atLeast ? value :
        //        throw new ArgumentException($"{parameterName} must be at least {atLeast}.", parameterName);
    }
}
