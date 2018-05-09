using System.Collections.Generic;
using Markdig;
using SharpYaml.Serialization;

namespace Deck.Rendering.Markdown
{
    public class SlideRenderer
    {
        private readonly Serializer _serializer = new Serializer();
        private readonly MarkdownPipeline _pipeline;

        public SlideRenderer()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.Use<HtmlSanitizerExtension>();
            _pipeline = pipelineBuilder.Build();
        }

        public Slide Render(string frontMatter, string markdown, string notes)
        {
            var metadata = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(frontMatter))
            {
                _serializer.DeserializeInto(frontMatter, metadata);
            }
            return new Slide
            {
                Metadata = metadata,
                Html = Markdig.Markdown.ToHtml(markdown, _pipeline),
                NotesHtml = string.IsNullOrWhiteSpace(notes) ? "" : Markdig.Markdown.ToHtml(notes, _pipeline)
            };
        }
    }
}
