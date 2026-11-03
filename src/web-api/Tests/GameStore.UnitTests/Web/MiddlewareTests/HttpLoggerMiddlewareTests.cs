using System.Text;
using GameStore.Web.Infrastructure.CustomMiddleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameStore.UnitTests.Web.MiddlewareTests
{
    public class HttpLoggerMiddlewareTests
    {
        private readonly Mock<ILogger<HttpLoggerMiddleware>> loggerMock;
        private readonly HttpLoggerMiddleware middleware;

        public HttpLoggerMiddlewareTests()
        {
            loggerMock = new Mock<ILogger<HttpLoggerMiddleware>>();
            middleware = new HttpLoggerMiddleware(loggerMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldCopyResponse_And_ResetRequestPosition()
        {
            var context = new DefaultHttpContext();

            var originalResponseBody = new MemoryStream();
            context.Response.Body = originalResponseBody;

            var requestContent = "{\"game\": \"doom\"}";
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));
            context.Request.Method = "POST";
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("localhost");
            context.Request.Path = "/api/games";

            var expectedResponse = "{\"status\":\"created\"}";

            async Task Next(HttpContext ctx)
            {
                ctx.Response.StatusCode = 201;
                await ctx.Response.WriteAsync(expectedResponse);
            }

            await middleware.InvokeAsync(context, Next);

            Assert.Equal(0, context.Request.Body.Position);

            originalResponseBody.Position = 0;
            var actualResponse = await new StreamReader(originalResponseBody).ReadToEndAsync();
            Assert.Equal(expectedResponse, actualResponse);

            Assert.Same(originalResponseBody, context.Response.Body);

            VerifyLoggerCalled(Times.Exactly(2), LogLevel.Information);
        }

        [Fact]
        public async Task InvokeAsync_WhenControllerThrowsException_ShouldStillRestoreStream()
        {
            var context = new DefaultHttpContext();
            var originalResponseBody = new MemoryStream();
            context.Response.Body = originalResponseBody;
            context.Request.Body = new MemoryStream();

            RequestDelegate next = (_) => throw new InvalidOperationException("Error!");

            await Assert.ThrowsAsync<InvalidOperationException>(() => middleware.InvokeAsync(context, next));

            Assert.Same(originalResponseBody, context.Response.Body);

            VerifyLoggerCalled(Times.Exactly(2), LogLevel.Information);
        }

        [Fact]
        public async Task InvokeAsync_WithEmptyRequestBody_ShouldNotCrash()
        {
            var context = new DefaultHttpContext();
            var originalResponseBody = new MemoryStream();
            context.Response.Body = originalResponseBody;

            RequestDelegate next = (_) => Task.CompletedTask;

            var exception = await Record.ExceptionAsync(() => middleware.InvokeAsync(context, next));

            Assert.Null(exception);
        }

        private void VerifyLoggerCalled(Times times, LogLevel expectedLogLevel)
        {
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == expectedLogLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                times);
        }
    }
}
