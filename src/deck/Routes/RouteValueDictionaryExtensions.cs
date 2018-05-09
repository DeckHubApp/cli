using Microsoft.AspNetCore.Routing;

namespace Deck.Routes
{
    internal static class RouteValueDictionaryExtensions
    {
        public static bool TryGetString(this RouteValueDictionary values, string key, out string value)
        {
            if (values.TryGetValue(key, out var obj))
            {
                value = (obj is string s) ? s : obj?.ToString();
                return true;
            }
            value = default(string);
            return false;
        }

        public static bool TryGetInt(this RouteValueDictionary values, string key, out int value)
        {
            if (values.TryGetValue(key, out var obj))
            {
                if (obj is int n)
                {
                    value = n;
                    return true;
                }

                if (int.TryParse(obj?.ToString() ?? "0", out value))
                {
                    return true;
                }
            }
            value = default(int);
            return false;
            
        }
    }
}