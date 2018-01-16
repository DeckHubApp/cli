namespace Slidey.Rendering.Markdown
{
    public static class BackgroundStyle
    {
        public static string Generate(string imageName) =>
            string.IsNullOrWhiteSpace(imageName)
                ? ""
                : $"background-image: url('/images/{imageName}'); background-size: cover";
    }
}