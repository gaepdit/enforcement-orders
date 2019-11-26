using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Enfo.Domain.Utils
{
    [DebuggerStepThrough]
    public static class Check
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string NotNullOrEmpty(string value, string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException($"{parameterName} can not be null or empty.", parameterName);
            }

            return value;
        }

        public static string NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"{parameterName} can not be null, empty, or white space.", parameterName);
            }

            return value;
        }

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException($"{parameterName} can not be null or empty.", parameterName);
            }

            return value;
        }

        public static int NotNegative(int value, string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentException($"{parameterName} can not be negative.", parameterName);
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

        //public static int AtLeast(int value, int atLeast, string parameterName)
        //{
        //    if (value < atLeast)
        //    {
        //        throw new ArgumentException($"{parameterName} must be at least {atLeast}.", parameterName);
        //    }

        //    return value;
        //}
    }
}
