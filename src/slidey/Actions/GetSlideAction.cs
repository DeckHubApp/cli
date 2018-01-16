using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using slidey.Embedded;
using Slidey.Rendering.Markdown;

namespace slidey.Actions
{
    public class GetSlideAction
    {
        private LiveShow _live;
        private readonly SlideyOptions _options;
        private readonly ISlideyClient _slideyClient;
        private readonly ILogger<GetSlideAction> _logger;

        public GetSlideAction(IServiceProvider serviceProvider)
        {
            _options = serviceProvider.GetRequiredService<IOptions<SlideyOptions>>().Value;
            _slideyClient = serviceProvider.GetRequiredService<ISlideyClient>();
            _logger = serviceProvider.GetRequiredService<ILogger<GetSlideAction>>();
        }

        public async Task Invoke(HttpRequest request, HttpResponse response, RouteData data)
        {
            if (data.Values.TryGetInt("index", out int index))
            {
                var show = await Slides.LoadAsync();
                if (_live == null)
                {
                    await StartOnline(show);
                }
                if (show.TryGetSlide(index, out var slide))
                {
                    var backgroundImage = slide.Metadata.GetStringOrDefault("backgroundImage", show.Metadata.GetStringOrEmpty("backgroundImage"));
                    response.ContentType = "text/html";
                    var html = Embedded.Web.template_html.Utf8ToString()
                        .Replace("{{title}}", slide.Metadata.GetStringOrDefault("title", show.Metadata.GetStringOrEmpty("title")))
                        .Replace("{{layout}}", slide.Metadata.GetStringOrDefault("layout", show.Metadata.GetStringOrDefault("layout", "blank")))
                        .Replace("{{inlineStyle}}", BackgroundStyle.Generate(backgroundImage))
                        .Replace("{{content}}", slide.Html)
                        .Replace("{{previousIndex}}", (index - 1).ToString(CultureInfo.InvariantCulture))
                        .Replace("{{nextIndex}}", (index + 1).ToString(CultureInfo.InvariantCulture))
                        .Replace("{{slidey}}", $"slidey.io/live/{_options.Presenter}/{_options.Slug}");

                    await response.WriteAsync(html).ConfigureAwait(false);
                    return;
                }
            }
            response.StatusCode = 404;
        }

        private async Task StartOnline(Show show)
        {
            if (_options.Offline) return;
            try
            {
                _live = await _slideyClient.StartShow(new StartShow
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