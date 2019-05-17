using System;
using System.Linq;

namespace Enfo.Models.Utils
{
    public static class StringFunctions
    {
        /// <summary>
        /// Implodes a String array to a single string, concatenating the items using the separator, and ignoring null or empty string items
        /// </summary>
        /// <param name="separator">The separator string to include between each item</param>
        /// <param name="items">An array of strings to concatenate</param>
        /// <returns>A concatenated string separated by the specified separator. Null or empty strings are ignored.</returns>
        /// <remarks></remarks>
        public static string ConcatNonEmptyStrings(string[] items, string separator)
        {
            return String.Join(separator, items.Where(s => !String.IsNullOrEmpty(s)));
        }
    }
}