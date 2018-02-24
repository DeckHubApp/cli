namespace SlideFace
{
    public class SlideFaceOptions
    {
        private bool _offline = true;
        public string Presenter { get; set; }
        public string Slug { get; set; }
        public string Place { get; set; }
        public string Api { get; set; }
        public string ApiKey { get; set; }

        public bool Offline
        {
            get => _offline || string.IsNullOrWhiteSpace(Api) || string.IsNullOrWhiteSpace(ApiKey);
            set => _offline = value;
        }
    }
}