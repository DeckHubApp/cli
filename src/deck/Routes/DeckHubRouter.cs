using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Deck.Embedded;
using Deck.Rendering.Markdown;

namespace Deck.Routes
{
    public static class DeckRouter
    {
        private static int _started;
        private static IDeckHubClient _client;
        private static DeckHubOptions _options;
        private static ILogger _logger;

        public static void Add(IRouteBuilder routes, IDeckHubClient client, DeckHubOptions options,
            ILoggerFactory loggerFactory)
        {
            _client = client;
            _options = options;
            _logger = loggerFactory.CreateLogger(typeof(DeckRouter));
            routes.MapGet("deckhub/{index}",
                (req, res, data) => data.Values.TryGetInt("index", out var index)
                    ? Get(res, index)
                    : res.NotFoundAsync());
        }

        private static async Task Get(HttpResponse res, int index)
        {
            var show = await Slides.LoadAsync();
            if (Interlocked.Exchange(ref _started, 1) == 0)
            {
                await StartOnline(show);
            }
            if (show.TryGetSlide(index, out var slide))
            {
                var backgroundImage = slide.Metadata.GetStringOrDefault("backgroundImage", show.Metadata.GetStringOrEmpty("backgroundImage"));
                var html = Web.template_html.Utf8ToString()
                    .Replace("{{title}}", slide.Metadata.GetStringOrDefault("title", show.Metadata.GetStringOrEmpty("title")))
                    .Replace("{{layout}}", slide.Metadata.GetStringOrDefault("layout", show.Metadata.GetStringOrDefault("layout", "blank")))
                    .Replace("{{inlineStyle}}", BackgroundStyle.Generate(backgroundImage))
                    .Replace("{{content}}", slide.Html)
                    .Replace("{{previousIndex}}", (index - 1).ToString(CultureInfo.InvariantCulture))
                    .Replace("{{nextIndex}}", (index + 1).ToString(CultureInfo.InvariantCulture))
                    .Replace("{{deckhub}}", _options.Api);

                res.ContentType = "text/html";
                res.StatusCode = 200;
                await res.WriteAsync(html);
                return;
            }

            res.StatusCode = 404;
        }
        
        private static async Task StartOnline(Show show)
        {
            if (_options.Offline)
            {
                return;
            }
            try
            {
                var startShow = new StartShow
                {
                    Presenter = _options.Presenter,
                    Place = _options.Place,
                    Slug = _options.Slug,
                    Time = DateTimeOffset.Now,
                    Title = _options.Title ?? show.Metadata.GetStringOrEmpty("title")
                };
                await _client.StartShow(startShow).ConfigureAwait(false);
                
                _logger.LogInformation($"Started: {startShow.Place} {startShow.Presenter} {startShow.Slug} {startShow.Time} {startShow.Title}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}