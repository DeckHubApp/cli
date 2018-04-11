// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using Microsoft.Extensions.Configuration;

namespace Slidable
{
    public class SlidableOptions
    {
        private bool _offline = true;
        public string Place { get; set; }
        public string Presenter { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Api { get; set; }
        public string ApiKey { get; set; }

        public bool Offline
        {
            get => _offline || string.IsNullOrWhiteSpace(Api) || string.IsNullOrWhiteSpace(ApiKey);
            set => _offline = value;
        }

        public static SlidableOptions Bind(IConfiguration configuration)
        {
            return new SlidableOptions
            {
                Api = configuration["Api"],
                ApiKey = configuration["ApiKey"],
                Offline = string.Equals(configuration["Offline"], "true", StringComparison.OrdinalIgnoreCase),
                Place = configuration["Place"],
                Presenter = configuration["Presenter"],
                Slug = configuration["Slug"]
            };
        }
    }
}