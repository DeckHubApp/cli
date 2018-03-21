using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Slidable.Embedded;

namespace Slidable.Routes
{
    internal static class HttpResponseExtensions
    {
        public static Task SendAsync(this HttpResponse response, ArraySegment<byte> content, string contentType)
        {
            response.StatusCode = 200;
            response.ContentType = contentType;
            return response.Body.WriteAsync(content);
            
        }

        public static Task NotFoundAsync(this HttpResponse response) => StatusCodeAsync(response, 404);

        public static Task StatusCodeAsync(this HttpResponse response, int statusCode)
        {
            response.StatusCode = statusCode;
            return Task.CompletedTask;
        }
    }
}