using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SlideFace.Rendering.Markdown;

namespace SlideFace
{
    public static class Slides
    {
        private static Show _instance;

        public static string Markdown { get; private set; }

        public static ValueTask<Show> LoadAsync()
        {
            return _instance != null
                ? new ValueTask<Show>(_instance)
                : new ValueTask<Show>(LoadImpl());
        }

        private static async Task<Show> LoadImpl()
        {
            var list = new List<Slide>();
            var path = Path.Combine(Environment.CurrentDirectory, "slides.md");
            if (!File.Exists(path))
            {
                return new Show(new Dictionary<string, object>(),  new List<Slide>(0));
            }
            var renderer = new ShowRenderer();
            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            {
                Markdown = await reader.ReadToEndAsync();
            }
            return _instance = renderer.Render(Markdown);
        }

        public static bool TryGetSlide(this Show show, int index, out Slide slide)
        {
            if (show.Slides.Count > index)
            {
                slide = show.Slides[index];
                return true;
            }
            slide = null;
            return false;
        }
    }
}