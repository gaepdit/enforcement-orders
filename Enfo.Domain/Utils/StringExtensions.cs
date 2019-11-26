using System.Diagnostics;

namespace Enfo.Domain.Utils
{
    [DebuggerStepThrough]
    public static class StringExtensions
    {
        /// <summary>
        /// Indicates whether this string is null or a System.String.Empty string.
        /// </summary>
        public static bool IsNullOrEmpty(this string str) =>
            string.IsNullOrEmpty(str);

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str) =>
            string.IsNullOrWhiteSpace(str);
    }
}
