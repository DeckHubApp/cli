using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace slidable.Controllers
{
    [Route("theme")]
    public class ThemeController
    {
        [HttpGet("{*path}")]
        public IActionResult Get(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var localParts = new string[parts.Length + 1];
            localParts[0] = "theme";
            parts.CopyTo(localParts, 1);
            var localPath = Path.Combine(localParts);
            if (File.Exists(localPath))
            {
                var extension = Path.GetExtension(localPath).TrimStart('.');
                var contentType = MediaTypes.TryGetValue(extension, out var mediaType)
                    ? mediaType
                    : $"text/{extension}";
                var stream = File.OpenRead(localPath);
                return new FileStreamResult(stream, contentType);
            }

            return new NotFoundResult();
        }

        private static readonly Dictionary<string, string> MediaTypes = 
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["css"] = "text/css",
                ["woff"] = "application/font-woff",
                ["woff2"] = "font/woff2",
                ["jpg"] = "image/jpeg",
                ["jpeg"] = "image/jpeg",
                ["png"] = "image/png",
                ["gif"] = "image/gif",
            };
    }
}