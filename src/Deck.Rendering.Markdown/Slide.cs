using System.Collections.Generic;

namespace Deck.Rendering.Markdown
{
    public class Slide
    {
        public Dictionary<string, object> Metadata { get; set; }
        public string Html { get; set; }
        public string NotesHtml { get; set; }
    }
}