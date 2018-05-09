using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Deck.Embedded;

namespace Deck.Routes
{
    public static class InternalFilesRouter
    {
        public static void Add(IRouteBuilder routes)
        {
            routes.MapGet("internal/{file}", (req, res, data) =>
            {
                if (data.Values.TryGetString("file", out var file))
                {
                    return Get(res, file);
                }

                return res.NotFoundAsync();
            });
        }

        private static Task Get(HttpResponse res, string file)
        {
            switch (file)
            {
                case "normalize.css":
                    return res.SendAsync(Web.normalize_css, "text/css");
                case "rasterize.js":
                    return res.SendAsync(Web.rasterize_js, "application/javascript");
                case "deck.js":
                    return res.SendAsync(Web.deck_js, "application/javascript");
                default:
                    return res.NotFoundAsync();
            }
        }
    }
}