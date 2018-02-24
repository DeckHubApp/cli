using System.Collections.Generic;
using System.Linq;

namespace SlideFace.Rendering.Markdown
{
    public class Show
    {
        public Show(IReadOnlyDictionary<string, object> metadata, IEnumerable<Slide> slides)
        {
            Metadata = metadata;
            Slides = slides.ToArray();
        }

        public IReadOnlyDictionary<string, object> Metadata { get; }

        public IReadOnlyList<Slide> Slides { get; }
    }
}