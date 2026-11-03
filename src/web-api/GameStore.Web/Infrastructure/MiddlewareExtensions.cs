using GameStore.Web.Infrastructure.CustomMiddleware;

namespace GameStore.Web.Infrastructure
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTotalGamesHeaderMiddleware(
            this IApplicationBuilder app)
            => app.UseMiddleware<TotalGamesHeaderMiddleware>();

        public static IApplicationBuilder UseHttpLoggerMiddleware(
            this IApplicationBuilder app)
            => app.UseMiddleware<HttpLoggerMiddleware>();
    }
}
