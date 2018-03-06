namespace Slidable
{
    public class LiveShow
    {
        public static readonly LiveShow Empty = new LiveShow();
        public string Presenter { get; set; }
        public string Slug { get; set; }
    }
}