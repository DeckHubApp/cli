using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Slidable.Routes
{
    public static class ThemeRouter
    {
        public static void Add(IRouteBuilder routes)
        {
            routes.MapGet("theme/{path}", (req, res, data) =>
            {
                if (data.Values.TryGetString("path", out var path))
                {
                    return Get(req, res, path);
                }

                return res.NotFoundAsync();
            });
        }

        private static Task Get(HttpRequest req, HttpResponse res, string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var localParts = new string[parts.Length + 1];
            localParts[0] = "theme";
            parts.CopyTo(localParts, 1);
            var localPath = Path.Combine(localParts);
            if (File.Exists(localPath))
            {
                var extension = Path.GetExtension(localPath).TrimStart('.');
                res.ContentType = MediaTypes.TryGetValue(extension, out var mediaType)
                    ? mediaType
                    : $"text/{extension}";
                res.StatusCode = 200;
                return res.SendFileAsync(localPath);
            }

            return res.NotFoundAsync();
        }
        
        private static readonly Dictionary<string, string> MediaTypes = 
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["css"] = "text/css",
                ["woff"] = "application/font-woff",
                ["woff2"] = "font/woff2",
                ["jpg"] = "image/jpeg",
                ["jpeg"] = "image/jpeg",
                ["png"] = "image/png",
                ["gif"] = "image/gif",
            };
    }
}