using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Slidable.Embedded;

namespace Slidable.Routes
{
    // Can't be static, need the logger
    // ReSharper disable once ConvertToStaticClass
    public sealed class ShotRouter
    {
        private static ISlidableClient _client;
        private static SlidableOptions _options;
        private static ILogger<ShotRouter> _logger;

        public static void Add(IRouteBuilder routes, ISlidableClient client, SlidableOptions options,
            ILoggerFactory loggerFactory)
        {
            _client = client;
            _options = options;
            _logger = loggerFactory.CreateLogger<ShotRouter>();
            routes.MapPost("shot/{index}", (req, res, data) =>
            {
                if (data.Values.TryGetInt("index", out var index))
                {
                    return Post(req, res, index);
                }

                return res.NotFoundAsync();
            });
        }

        private static async Task Post(HttpRequest req, HttpResponse res, int index)
        {
            try
            {
                await _client.SetShown(_options.Presenter, _options.Slug, index, req.Body, req.ContentType);
                res.StatusCode = 201;
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting screenshot");
            }

            res.StatusCode = 500;
        }

        private ShotRouter() { }
    }
}