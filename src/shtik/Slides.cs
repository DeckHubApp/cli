using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shtik.Rendering.Markdown;

namespace shtik
{
    public class Slides
    {
        private static Slides _instance;

        private readonly List<Slide> _slides;

        private Slides(List<Slide> slides)
        {
            _slides = slides;
        }

        public static ValueTask<Slides> LoadAsync()
        {
            return _instance != null
                ? new ValueTask<Slides>(_instance)
                : new ValueTask<Slides>(LoadImpl());
        }

        public bool TryGet(int index, out Slide slide)
        {
            if (_slides.Count >= index)
            {
                slide = _slides[index - 1];
                return true;
            }
            slide = null;
            return false;
        }

        public int Count => _slides.Count;

        private static async Task<Slides> LoadImpl()
        {
            var list = new List<Slide>();
            var path = Path.Combine(Environment.CurrentDirectory, "slides.md");
            if (!File.Exists(path))
            {
                return new Slides(new List<Slide>(0));
            }
            var renderer = new Renderer();
            using (var stream = File.OpenRead(path))
            {
                var splitter = new Splitter(stream);
                while (true)
                {
                    var frontMatter = await splitter.ReadNextBlockAsync();
                    if (frontMatter == null) break;
                    var markdown = await splitter.ReadNextBlockAsync();
                    if (markdown == null) break;
                    var slide = renderer.Render(frontMatter, markdown);
                    list.Add(slide);
                }
            }
            return _instance = new Slides(list);
        }
    }
}