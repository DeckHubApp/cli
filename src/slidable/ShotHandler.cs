using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Slidable
{
    public static class ShotHandler
    {
        public static async Task Handle(HttpRequest req, HttpResponse res, RouteData data)
        {
            if (Things.SlidableOptions.Offline)
            {
                res.StatusCode = 201;
                return;
            }

            if (data.Values.TryGetValue("index", out var indexArg))
            {
                if (int.TryParse(indexArg.ToString(), out int index))
                {
                    try
                    {
                        await Things.SlidableClient.SetShown(Things.SlidableOptions.Presenter,
                            Things.SlidableOptions.Slug, index, req.Body,
                            req.ContentType);
                        res.StatusCode = 201;
                        return;
                    }
                    catch (Exception ex)
                    {
                        Things.LoggerFactory.CreateLogger("Shot").LogError(ex, "Error posting screenshot");
                    }

                    res.StatusCode = 500;
                }
            }
        }
    }
}