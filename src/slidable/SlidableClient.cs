﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Slidable
{
    public class SlidableClient : ISlidableClient
    {
        private readonly ILogger<SlidableClient> _logger;
        private readonly HttpClient _http;

        public SlidableClient(SlidableOptions options, ILogger<SlidableClient> logger)
        {
            _logger = logger;
            if (!options.Offline)
            {
                _http = new HttpClient
                {
                    BaseAddress = new Uri(options.Api),
                };
                _http.DefaultRequestHeaders.Add("API-Key", options.ApiKey);
            }
        }

        public async Task<LiveShow> StartShow(StartShow start)
        {
            var json = JsonConvert.SerializeObject(start);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var requestUri = $"/present/{start.Presenter}/start";
            Console.WriteLine($"Starting show at {requestUri}");
            var response = await _http.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error starting online show: {statusCode} - {reason}", response.StatusCode, response.ReasonPhrase);
                return null;
            }
            json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LiveShow>(json);
        }

        public Task SetShown(string presenter, string slug, int index, Stream slide, string contentType)
        {
            return Task.WhenAll(
                SetSlideShown(presenter, slug, index),
                UploadSlideImage(presenter, slug, index, slide, contentType)
            );
        }

        private async Task SetSlideShown(string presenter, string slug, int index)
        {
            var content = new StringContent(string.Empty);
            var response = await _http.PutAsync($"/present/{presenter}/{slug}/{index}", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error showing slide {index}: {statusCode} - {reason}",
                    index, response.StatusCode, response.ReasonPhrase);
            }
        }

        private async Task UploadSlideImage(string presenter, string slug, int index, Stream slide, string contentType)
        {
            var content = new StreamContent(slide);
            content.Headers.ContentType = MediaTypeHeaderValue.TryParse(contentType, out var mediaType)
                ? mediaType
                : MediaTypeHeaderValue.Parse("application/octet-stream");

            var response = await _http.PutAsync($"/slides/{presenter}/{slug}/{index}", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error uploading slide {index}: {statusCode} - {reason}",
                    index, response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}