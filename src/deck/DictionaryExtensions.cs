using System.Collections.Generic;

namespace Deck
{
    internal static class DictionaryExtensions
    {
        public static string GetStringOrEmpty(this IReadOnlyDictionary<string, object> dictionary, string key)
        {
            return GetStringOrDefault(dictionary, key, string.Empty);
        }

        public static string GetStringOrDefault(this IReadOnlyDictionary<string, object> dictionary, string key, string defaultValue)
        {
            if (dictionary.TryGetValue(key, out var obj))
            {
                return (obj is string s) ? s : obj?.ToString() ?? defaultValue;
            }
            return defaultValue;
        }
    }
}