using System.Collections.Generic;
using SharpYaml.Serialization;

namespace Shtik.Rendering.Markdown
{
    public class Renderer
    {
        private readonly Serializer _serializer = new Serializer();

        public Slide Render(string frontMatter, string markdown)
        {
            var metadata = new Dictionary<string, object>();
            _serializer.DeserializeInto(frontMatter, metadata);
            return new Slide
            {
                Metadata = metadata,
                Html = Markdig.Markdown.ToHtml(markdown)
            };
        }
    }
}
