using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Infrastructure
{
    internal class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";

            response.StatusCode = StatusCodes.Status500InternalServerError;

            logger.LogError("Global exception handler caught an error:\n" +
                                       "\tException type: {Type}\n" +
                                       "\tSource: {Source}\n" +
                                       "\tException message: {Message}\n" +
                                       "\tInner exception: {InnerException}\n" +
                                       "\tStack Trace: {StackTrace}\n"
                , exception.GetType().ToString(), exception.Source, exception.Message, 
                exception.InnerException, exception.ToString());

            var problemDetails = new ProblemDetails()
            {
                Type = exception.GetType().Name,
                Status = response.StatusCode,
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
