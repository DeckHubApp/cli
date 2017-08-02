using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shtik.Rendering.Markdown;

namespace shtik
{
    public static class Slides
    {
        private static List<Slide> _cache;
        public static ValueTask<(bool, Slide)> TryGet(int index)
        {
            if (_cache != null)
            {
                if (_cache.Count > index)
                {
                    return Result(true, _cache[index]);
                }
                else
                {
                    return Result(false);
                }
            }

            return new ValueTask<(bool, Slide)>(Load(index));
        }

        private static async Task<(bool, Slide)> Load(int index)
        {
            _cache = new List<Slide>();
            var path = Path.Combine(Environment.CurrentDirectory, "slides.md");
            if (!File.Exists(path))
            {
                return (false, null);
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
                    _cache.Add(slide);
                }
            }
            if (_cache.Count > index)
            {
                return (true, _cache[index]);
            }
            return (false, null);
        }

        private static ValueTask<(bool, Slide)> Result(bool found, Slide slide = null)
        {
            return new ValueTask<(bool, Slide)>((found, slide));
        }
    }
}