using GameStore.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Infrastructure.Results
{
    public static class ApiResults
    {
        public static IActionResult Problem(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot convert a successful result to a problem.");
            }

            var problemDetails = new ProblemDetails
            {
                Title = result.Error.Code,
                Detail = result.Error.Description,
                Status = GetStatusCode(result.Error.Type)
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }

        private static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.InvalidData => StatusCodes.Status400BadRequest,
                ErrorType.Failure => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
