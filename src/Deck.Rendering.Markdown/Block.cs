namespace Deck.Rendering.Markdown
{
    public readonly struct Block
    {
        public readonly string FrontMatter;
        public readonly string Slide;
        public readonly string Notes;

        public Block(string frontMatter, string slide, string notes)
        {
            FrontMatter = frontMatter?.Trim();
            Slide = slide?.Trim();
            Notes = notes?.Trim();
        }
    }
}