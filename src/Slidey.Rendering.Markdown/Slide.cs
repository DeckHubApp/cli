using System.Collections.Generic;

namespace Slidey.Rendering.Markdown
{
    public class Slide
    {
        public Dictionary<string, object> Metadata { get; set; }
        public string Html { get; set; }
    }
}