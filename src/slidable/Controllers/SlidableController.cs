using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slidable;
using Slidable.Embedded;
using Slidable.Rendering.Markdown;

namespace slidable.Controllers
{
    [Route("slidable")]
    public class SlidableController
    {
        private static int _started = 0;
        private static LiveShow _live;
        private readonly SlidableOptions _options;
        private readonly ISlidableClient _client;
        private readonly ILogger<SlidableController> _logger;

        public SlidableController()
        {
            _logger = Things.LoggerFactory.CreateLogger<SlidableController>();
            _options = Things.SlidableOptions;
            _client = Things.SlidableClient;
        }

        [HttpGet("{index}")]
        public IActionResult Get(int index)
        {
            return GetImpl(index.ToString()).GetAwaiter().GetResult();
        }

        private async Task<IActionResult> GetImpl(string index)
        {
            if (!int.TryParse(index, out int number))
            {
                return new NotFoundResult();
            }
            var show = await Slides.LoadAsync();//.GetAwaiter().GetResult();
            if (Interlocked.Exchange(ref _started, 1) == 0)
            {
                await StartOnline(show);//.GetAwaiter().GetResult();
            }
            if (show.TryGetSlide(number, out var slide))
            {
                var backgroundImage = slide.Metadata.GetStringOrDefault("backgroundImage", show.Metadata.GetStringOrEmpty("backgroundImage"));
                var html = Web.template_html.Utf8ToString()
                    .Replace("{{title}}", slide.Metadata.GetStringOrDefault("title", show.Metadata.GetStringOrEmpty("title")))
                    .Replace("{{layout}}", slide.Metadata.GetStringOrDefault("layout", show.Metadata.GetStringOrDefault("layout", "blank")))
                    .Replace("{{inlineStyle}}", BackgroundStyle.Generate(backgroundImage))
                    .Replace("{{content}}", slide.Html)
                    .Replace("{{previousIndex}}", (number - 1).ToString(CultureInfo.InvariantCulture))
                    .Replace("{{nextIndex}}", (index + 1).ToString(CultureInfo.InvariantCulture))
                    .Replace("{{slidable}}", _options.Api);

                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = html
                };
            }

            return new NotFoundResult();
        }

        private async Task StartOnline(Show show)
        {
            if (_options.Offline)
            {
                return;
            }
            try
            {
                _live = await _client.StartShow(new StartShow
                {
                    Markdown = Slides.Markdown,
                    Presenter = _options.Presenter,
                    Place = _options.Place,
                    Slug = _options.Slug,
                    Time = DateTimeOffset.Now,
                    Title = show.Metadata.GetStringOrEmpty("title")
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _live = LiveShow.Empty;
            }
        }
    }
}