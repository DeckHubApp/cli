using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Slidable
{
    [Route("internal")]
    public class InternalFilesController
    {
        [HttpGet("normalize.css")]
        public IActionResult NormalizeCss()
        {
            return new ContentResult()
            {
                ContentType = "text/css; charset=utf-8",
                Content = Encoding.UTF8.GetString(Embedded.Web.normalize_css.ToArray())
            };
        }

        [HttpGet("rasterize.js")]
        public IActionResult RasterizeJs()
        {
            return new ContentResult()
            {
                ContentType = "text/javascript; charset=utf-8",
                Content = Encoding.UTF8.GetString(Embedded.Web.rasterize_js.ToArray())
            };
        }

        [HttpGet("slidable.js")]
        public IActionResult SlidableJs()
        {
            return new ContentResult()
            {
                ContentType = "text/javascript; charset=utf-8",
                Content = Encoding.UTF8.GetString(Embedded.Web.slidable_js.ToArray())
            };
        }
    }
}