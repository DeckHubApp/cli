using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Slidable
{
    public static class Things
    {
        public static IHttpContextAccessor HttpContextAccessor { get; set; }
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ISlidableClient SlidableClient { get; set; }
        public static SlidableOptions SlidableOptions { get; set; }
    }
}