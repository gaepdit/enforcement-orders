using System.Diagnostics;

namespace Enfo.Domain.Utils
{
    [DebuggerStepThrough]
    public static class StringExtensions
    {
        /// <summary>
        /// Indicates whether this string is null or a System.String.Empty string.
        /// </summary>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    }
}
