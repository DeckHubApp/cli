using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using shtik.Actions;
using Shtik.Rendering.Markdown;

namespace shtik
{
    public static class Routes
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("shtik.Properties.Resources", typeof(Routes).Assembly);

        public static void Router(IRouteBuilder routes)
        {
            routes.MapGet("normalize.css", (request, response, data) =>
            {
                response.ContentType = "text/css";
                return response.WriteAsync(Properties.Resources.normalize_css);
            });

            routes.MapGet("theme.css", (request, response, data) =>
            {
                response.ContentType = "text/css";
                return response.WriteAsync(Properties.Resources.theme_css);
            });

            routes.MapGet("shtik.js", (request, response, data) =>
            {
                response.ContentType = "text/javascript";
                return response.WriteAsync(Properties.Resources.shtik_js);
            });

            routes.MapGet("fonts/{font}", (request, response, data) =>
            {
                if (data.Values.TryGetString("font", out string font))
                {
                    response.ContentType = font.EndsWith(".woff") ? "application/font-woff" : "font/woff2";
                    var resourceName = font.Replace(".woff", "_woff").Replace('-', '_');
                    if (ResourceManager.GetObject(resourceName) is byte[] bytes)
                    {
                        response.ContentLength = bytes.Length;
                        return response.Body.WriteAsync(bytes, 0, bytes.Length);
                    }
                }
                response.StatusCode = 404;
                return Task.CompletedTask;
            });

            routes.MapGet("{index}", new GetSlideAction(routes.ServiceProvider).Invoke);
        }
    }
}
