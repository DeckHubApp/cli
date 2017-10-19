using System;
using System.IO;
using System.Threading.Tasks;
using shtik.Embedded;

namespace shtik.Commands
{
    public class NewCommand
    {
        private readonly string[] _args;

        public NewCommand(string[] args)
        {
            _args = args;
        }

        public async Task Execute()
        {
            if (File.Exists("slides.md"))
            {
                Console.Error.WriteLine(
                    "A presentation already exists in this directory, and I don't want to overwrite it in case it's important.");
                return;
            }

            await Write(Empty.slides_md, "slides.md");
            await Write(Web.theme_css, "theme", "theme.css");
            await Write(Web.fonts_fira_sans_v7_latin_700_woff, "theme", "fonts", "fira-sans-v7-latin-700.woff");
            await Write(Web.fonts_fira_sans_v7_latin_700_woff2, "theme", "fonts", "fira-sans-v7-latin-700.woff2");
            await Write(Web.fonts_fira_sans_v7_latin_700italic_woff, "theme", "fonts", "fira-sans-v7-latin-700italic.woff");
            await Write(Web.fonts_fira_sans_v7_latin_700italic_woff2, "theme", "fonts", "fira-sans-v7-latin-700italic.woff2");
            await Write(Web.fonts_lato_v13_latin_regular_woff, "theme", "fonts", "lato-v13-latin-regular.woff");
            await Write(Web.fonts_lato_v13_latin_regular_woff2, "theme", "fonts", "lato-v13-latin-regular.woff2");
            await Write(Web.fonts_lato_v13_latin_italic_woff, "theme", "fonts", "lato-v13-latin-italic.woff");
            await Write(Web.fonts_lato_v13_latin_italic_woff2, "theme", "fonts", "lato-v13-latin-italic.woff2");

            Console.WriteLine("Done. Edit slides.md to create your Shtik presentation.");
        }

        private async Task Write(ArraySegment<byte> content, params string[] pathBits)
        {
            var filePath = Path.Combine(pathBits);
            if (pathBits.Length > 1)
            {
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }

            if (!File.Exists(filePath))
            {
                using (var stream = File.Create(filePath))
                {
                    await stream.WriteAsync(content);
                }
                Console.WriteLine($"{filePath}.");
            }
            else
            {
                Console.WriteLine($"{filePath} already exists.");
            }
        }
    }
}