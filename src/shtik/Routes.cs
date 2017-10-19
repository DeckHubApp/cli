using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using shtik.Actions;

namespace shtik
{
    public static class Routes
    {
        public static void Router(IRouteBuilder routes)
        {
            routes.MapGet("normalize.css", (request, response, data) =>
            {
                response.ContentType = "text/css; charset=utf-8";
                return response.Body.WriteAsync(Embedded.Web.normalize_css);
            });

            routes.MapGet("rasterize.js", (request, response, data) =>
            {
                response.ContentType = "text/javascript; charset=utf-8";
                return response.Body.WriteAsync(Embedded.Web.rasterize_js);
            });

            routes.MapGet("shtik.js", (request, response, data) =>
            {
                response.ContentType = "text/javascript; charset=utf-8";
                return response.Body.WriteAsync(Embedded.Web.shtik_js);
            });

            routes.MapGet("theme/{path*}", (request, response, data) =>
            {
                if (data.Values.TryGetString("path", out var path))
                {
                    var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    var localParts = new string[parts.Length + 1];
                    localParts[0] = "theme";
                    parts.CopyTo(localParts, 1);
                    var localPath = Path.Combine(localParts);
                    if (File.Exists(localPath))
                    {
                        var extension = Path.GetExtension(localPath).TrimStart('.');
                        response.ContentType = MediaTypes.TryGetValue(extension, out var mediaType)
                            ? mediaType
                            : $"text/{extension}";
                        return response.SendFileAsync(localPath, request.HttpContext.RequestAborted);
                    }
                }
                response.StatusCode = 404;
                return Task.CompletedTask;
            });

            routes.MapGet("{index}", new GetSlideAction(routes.ServiceProvider).Invoke);

            routes.MapPost("shot/{index}", new UploadSlideAction(routes.ServiceProvider).Invoke);
        }

        private static readonly Dictionary<string, string> MediaTypes = 
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["css"] = "text/css",
                ["woff"] = "application/font-woff",
                ["woff2"] = "font/woff2",
            };
    }
}
