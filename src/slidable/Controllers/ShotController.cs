using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Slidable;

namespace slidable.Controllers
{
    //[Route("shot")]
    public class ShotController
    {
        private readonly SlidableOptions _options;
        private readonly ISlidableClient _client;
        private readonly ILogger<ShotController> _logger;
        
        public ShotController()
        {
            _options = Things.SlidableOptions;
            _client = Things.SlidableClient;
            _logger = Things.LoggerFactory.CreateLogger<ShotController>();
        }

        //[HttpPost("{index}")]
        public IActionResult Post(int index)
        {
            return PostImpl(index, Things.HttpContextAccessor.HttpContext).GetAwaiter().GetResult();
        }

        private async Task<IActionResult> PostImpl(int index, HttpContext context)
        {
            if (_options.Offline)
            {
                return new StatusCodeResult(201);
            }

            try
            {
                await _client.SetShown(_options.Presenter, _options.Slug, index, context.Request.Body,
                    context.Request.ContentType);
                return new StatusCodeResult(201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting screenshot");
            }
            return new StatusCodeResult(500);
        }
    }
}