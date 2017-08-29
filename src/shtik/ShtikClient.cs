using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shtik.Rendering.Markdown;

namespace shtik
{
    public class ShtikClient : IShtikClient
    {
        private readonly ILogger<ShtikClient> _logger;
        private readonly HttpClient _http;

        public ShtikClient(IOptions<ShtikOptions> options, ILogger<ShtikClient> logger)
        {
            _logger = logger;
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Api),
            };
            _http.DefaultRequestHeaders.Add("API-Key", options.Value.ApiKey);
        }

        public async Task<LiveShow> StartShow(StartShow start)
        {
            var json = JsonConvert.SerializeObject(start);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync($"/present/{start.Presenter}/start", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error starting online show: {statusCode} - {reason}", response.StatusCode, response.ReasonPhrase);
                return null;
            }
            json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LiveShow>(json);
        }

        public async Task<bool> SetShown(LiveShow show, int index)
        {
            var content = new StringContent(string.Empty);
            var response = await _http.PutAsync($"/present/{show.Presenter}/{show.Slug}/{index}", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error starting online show: {statusCode} - {reason}", response.StatusCode, response.ReasonPhrase);
            }
            return response.IsSuccessStatusCode;
        }
    }

    public class LiveShow
    {
        public string Presenter { get; set; }
        public string Slug { get; set; }
    }

    public class StartShow
    {
        public string Presenter { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Place { get; set; }
        public string Markdown { get; set; }
    }
}