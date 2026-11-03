using GameStore.Core.Interfaces.Services;
using GameStore.Web.Infrastructure.CustomMiddleware;
using GameStore.Web.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

namespace GameStore.UnitTests.Web.MiddlewareTests
{
    public class TotalGamesHeaderMiddlewareTests
    {
        private readonly Mock<IGameService> gameServiceMock;
        private readonly IOptionsSnapshot<CacheOptions> options;
        private readonly IMemoryCache cache;
        private readonly DefaultHttpContext httpContext;
        private readonly TotalGamesHeaderMiddleware middleware;

        public TotalGamesHeaderMiddlewareTests()
        {
            gameServiceMock = new Mock<IGameService>();

            var optionsMock = new Mock<IOptionsSnapshot<CacheOptions>>();
            optionsMock.Setup(m => m.Value).Returns(new CacheOptions
            {
                TotalGamesKey = "TestKey",
                DurationSeconds = 300
            });
            options = optionsMock.Object;
            cache = new MemoryCache(new MemoryCacheOptions());
            httpContext = new DefaultHttpContext();

            middleware = new TotalGamesHeaderMiddleware(cache, gameServiceMock.Object, options);
        }

        [Fact]
        public async Task InvokeAsync_WhenCacheIsEmpty_ShouldCallService_AndSetHeader()
        {
            int expectedCount = 42;
            gameServiceMock
                .Setup(s => s.GetGameCount(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCount);

            Task Next(HttpContext _) => Task.CompletedTask;

            await middleware.InvokeAsync(httpContext, Next);

            Assert.True(httpContext.Response.Headers.ContainsKey("x-total-numbers-of-games"));
            Assert.Equal(expectedCount.ToString(), httpContext.Response.Headers["x-total-numbers-of-games"]);
            gameServiceMock.Verify(s => s.GetGameCount(It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(cache.TryGetValue(options.Value.TotalGamesKey, out int cachedCount));
            Assert.Equal(expectedCount, cachedCount);
        }

        [Fact]
        public async Task InvokeAsync_WhenCacheHasData_ShouldNotCallService_AndSetHeader()
        {
            int cachedCount = 99;
            cache.Set(options.Value.TotalGamesKey, cachedCount);

            Task Next(HttpContext _) => Task.CompletedTask;

            await middleware.InvokeAsync(httpContext, Next);

            Assert.True(httpContext.Response.Headers.ContainsKey("x-total-numbers-of-games"));
            Assert.Equal(cachedCount.ToString(), httpContext.Response.Headers["x-total-numbers-of-games"]);
            gameServiceMock.Verify(s => s.GetGameCount(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
