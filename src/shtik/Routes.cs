using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

            routes.MapGet("script.js", (request, response, data) =>
            {
                response.ContentType = "text/javascript";
                return response.WriteAsync(Properties.Resources.script_js);
            });

            routes.MapGet("fonts/{font}", (request, response, data) =>
            {
                if (data.Values.TryGetString("font", out string font))
                {
                    response.ContentType = font.EndsWith(".woff") ? "application/font-woff" : "font/woff2";
                    var resourceName = font.Replace(".woff", "_woff").Replace('-', '_');
                    var bytes = ResourceManager.GetObject(resourceName) as byte[];
                    if (bytes != null)
                    {
                        response.ContentLength = bytes.Length;
                        return response.Body.WriteAsync(bytes, 0, bytes.Length);
                    }
                }
                response.StatusCode = 404;
                return Task.CompletedTask;
            });

            routes.MapGet("{index}", async (request, response, data) =>
            {
                var options = request.HttpContext.RequestServices.GetRequiredService<IOptions<ShtikOptions>>().Value;

                if (data.Values.TryGetInt("index", out int index))
                {
                    var slides = await Slides.LoadAsync();
                    if (slides.TryGet(index, out var slide))
                    {
                        response.ContentType = "text/html";
                        var html = Properties.Resources.template_html
                            .Replace("{{title}}", slide.Metadata.GetStringOrEmpty("title"))
                            .Replace("{{layout}}", slide.Metadata.GetStringOrDefault("layout", "blank"))
                            .Replace("{{content}}", slide.Html)
                            .Replace("{{previousIndex}}", (index - 1).ToString(CultureInfo.InvariantCulture))
                            .Replace("{{nextIndex}}", (index + 1).ToString(CultureInfo.InvariantCulture))
                            .Replace("{{shtik}}", $"shtik.io/live/{options.Presenter}/{options.Slug}");
                        await response.WriteAsync(html);
                        return;
                    }
                }
                response.StatusCode = 404;
            });
        }
    }
}
