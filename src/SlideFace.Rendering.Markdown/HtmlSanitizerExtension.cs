using System.Linq;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace SlideFace.Rendering.Markdown
{
    public class HtmlSanitizerExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (!(renderer is HtmlRenderer htmlRenderer)) return;

            var blockRenderer = htmlRenderer.ObjectRenderers.FindExact<HtmlBlockRenderer>();
            blockRenderer?.TryWriters.AddIfNotAlready<MarkdownObjectRenderer<HtmlRenderer, HtmlBlock>.TryWriteDelegate>(TryScriptBlockRenderer);

            var inlineRenderer = htmlRenderer.ObjectRenderers.FindExact<HtmlInlineRenderer>();
            inlineRenderer?.TryWriters.AddIfNotAlready<MarkdownObjectRenderer<HtmlRenderer, HtmlInline>.TryWriteDelegate>(TryScriptInlineRenderer);
        }

        private static bool TryScriptInlineRenderer(HtmlRenderer renderer, HtmlInline inline)
        {
            if (!inline.Tag.Contains("script")) return false;

            renderer.WriteEscape(inline.Tag);
            return true;
        }

        private static bool TryScriptBlockRenderer(HtmlRenderer renderer, HtmlBlock block)
        {
            if (!block.Lines.Lines.Any(l => l.Slice.Text.Contains("script"))) return false;

            foreach (var line in block.Lines.Lines)
            {
                renderer.WriteEscape(line.Slice);
            }
            return true;
        }
    }
}