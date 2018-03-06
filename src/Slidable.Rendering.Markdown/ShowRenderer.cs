using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Slidable.Rendering.Markdown
{
    public class ShowRenderer
    {
        private readonly Serializer _serializer = new Serializer();
        private readonly SlideRenderer _slideRenderer = new SlideRenderer();

        public Show Render(string source)
        {
            var splitter = new Splitter(source);
            var frontMatter = splitter.ReadFrontMatter();
            var showMetadata = new Dictionary<string, object>();
            _serializer.DeserializeInto(frontMatter, showMetadata);

            return new Show(showMetadata, RenderSlides(splitter));
        }

        private IEnumerable<Slide> RenderSlides(Splitter splitter)
        {
            while (true)
            {
                var (frontMatter, markdown) = splitter.ReadNextBlock();
                if (string.IsNullOrWhiteSpace(frontMatter) && string.IsNullOrWhiteSpace(markdown)) yield break;
                yield return _slideRenderer.Render(frontMatter, markdown);
            }
        }
    }
}