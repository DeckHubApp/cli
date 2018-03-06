using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Slidable.Actions
{
    public class UploadSlideAction
    {
        private readonly SlidableOptions _options;
        private readonly ISlidableClient _slidableClient;
        private readonly ILogger<UploadSlideAction> _logger;

        public UploadSlideAction(IServiceProvider services)
        {
            _options = services.GetRequiredService<IOptions<SlidableOptions>>().Value;
            _slidableClient = services.GetRequiredService<ISlidableClient>();
            _logger = services.GetRequiredService<ILogger<UploadSlideAction>>();
        }

        public async Task Invoke(HttpRequest request, HttpResponse response, RouteData data)
        {
            if (_options.Offline)
            {
                response.StatusCode = 200;
                return;
            }
            if (data.Values.TryGetInt("index", out var index))
            {
                if (await _slidableClient.SetShown(_options.Presenter, _options.Slug, index, request.Body, request.ContentType))
                {
                    response.StatusCode = 201;
                }
                else
                {
                    response.StatusCode = 500;
                }
            }
            else
            {
                response.StatusCode = 400;
            }
        }
    }
}