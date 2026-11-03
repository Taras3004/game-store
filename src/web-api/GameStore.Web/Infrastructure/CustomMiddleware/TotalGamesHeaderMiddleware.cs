using GameStore.Core.Interfaces.Services;
using GameStore.Web.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GameStore.Web.Infrastructure.CustomMiddleware
{
    public class TotalGamesHeaderMiddleware(IMemoryCache cache, IGameService gameService, IOptionsSnapshot<CacheOptions> cacheOptions) : IMiddleware
    {
        private readonly CacheOptions options = cacheOptions.Value;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!cache.TryGetValue(options.TotalGamesKey, out int totalGames))
            {
                totalGames = await gameService.GetGameCount();

                cache.Set(
                    options.TotalGamesKey, 
                    totalGames,
                    TimeSpan.FromSeconds(options.DurationSeconds));
            }

            context.Response.Headers.Append("x-total-numbers-of-games", totalGames.ToString());

            await next(context);
        }
    }
}