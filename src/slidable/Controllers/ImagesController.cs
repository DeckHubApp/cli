using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace slidable.Controllers
{
    [Route("images")]
    public class ImagesController
    {
        [HttpGet("{*path}")]
        public IActionResult Get(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var localParts = new string[parts.Length + 1];
            localParts[0] = "images";
            parts.CopyTo(localParts, 1);
            var localPath = Path.Combine(localParts);
            if (File.Exists(localPath))
            {
                var extension = Path.GetExtension(localPath).TrimStart('.');
                if (extension.Equals("jpg", StringComparison.OrdinalIgnoreCase))
                {
                    extension = "jpeg";
                }
                var contentType = $"image/{extension}";
                var stream = File.OpenRead(localPath);
                return new FileStreamResult(stream, contentType);
            }
            return new NotFoundResult();
        }
    }
}