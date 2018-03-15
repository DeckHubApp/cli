using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Slidable.Controllers
{
    [Route("")]
    public class HomeController
    {
        // GET
        [HttpGet("normalize.css")]
        public IActionResult Index()
        {
            return new ContentResult()
            {
                ContentType = "text/css; charset=utf-8",
                Content = Encoding.UTF8.GetString(Embedded.Web.normalize_css.ToArray())
            };
        }
    }
}