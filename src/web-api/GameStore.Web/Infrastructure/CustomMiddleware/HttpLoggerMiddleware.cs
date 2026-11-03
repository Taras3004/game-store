using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;

namespace GameStore.Web.Infrastructure.CustomMiddleware
{
    public class HttpLoggerMiddleware(ILogger<HttpLoggerMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;
            string body;

            context.Request.EnableBuffering();

            using (var reader = new StreamReader(
                       context.Request.Body,
                       Encoding.UTF8,
                       leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
            }

            logger.LogInformation("\n\tRequest type: {Type}\n" +
                                  "\tUrl: {Url}\n" +
                                  "\tUser IP address: {Address}\n" +
                                  "\tRequest body: {Body}",
                context.Request.Method, context.Request.GetDisplayUrl(),
                context.Connection.RemoteIpAddress?.ToString(), body);
            context.Request.Body.Position = 0;

            using var responseBodyMemoryStream = new MemoryStream();
            context.Response.Body = responseBodyMemoryStream;

            try
            {
                await next(context);
            }
            finally
            {
                sw.Stop();

                responseBodyMemoryStream.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(responseBodyMemoryStream).ReadToEndAsync();

                logger.LogInformation("\n\tResponded in {Time} with status code {StatusCode}\n" +
                                      "\tResponse Body: {Body}", sw.Elapsed,
                    context.Response.StatusCode, responseBodyText);

                responseBodyMemoryStream.Seek(0, SeekOrigin.Begin);

                await responseBodyMemoryStream.CopyToAsync(originalBodyStream);

                context.Response.Body = originalBodyStream;
            }
        }
    }
}