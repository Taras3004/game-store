using GameStore.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Infrastructure.Results
{
    public static class ResultExtensions
    {
        public static IActionResult Match(
            this Result result,
            Func<IActionResult> onSuccess,
            Func<Result, IActionResult> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result);
        }

        public static IActionResult Match<T>(
            this Result<T> result,
            Func<T, IActionResult> onSuccess,
            Func<Result, IActionResult> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
        }
    }
}
