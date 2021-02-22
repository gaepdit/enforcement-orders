using System.Collections.Generic;
using System.Linq;

namespace Enfo.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Indicates whether this string is null or a System.String.Empty string.
        /// </summary>
        public static bool IsNullOrEmptyString(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpaceString(this string str) => string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// Implodes a String array to a single string, concatenating the items using the separator
        /// and ignoring null or empty string items
        /// </summary>
        /// <param name="separator">The separator string to include between each item</param>
        /// <param name="items">An array of strings to concatenate</param>
        /// <returns>A concatenated string separated by the specified separator.
        /// Null or empty strings are ignored.</returns>
        public static string ConcatNonEmptyStrings(this IEnumerable<string> items, string separator) =>
            string.Join(separator, items.Where(s => !s.IsNullOrEmptyString()));
    }
}
