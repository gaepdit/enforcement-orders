using System.Collections.Generic;
using System.Linq;

namespace Enfo.Domain.Utils
{
    public static class DictionaryExtensions
    {
        public static string DictionaryToString(this Dictionary<string, string> items) => 
            string.Join(", ", items.Select(kv => $"{kv.Key}: {kv.Value}"));
    }
}
