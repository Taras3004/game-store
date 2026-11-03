using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Payment.Functions.Http;

internal static class RequestBody
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public static async Task<BodyResult<T>> ReadAsync<T>(HttpRequest request, CancellationToken cancellationToken)
        where T : class
    {
        if (!request.HasJsonContentType())
        {
            return new BodyResult<T>(null, new StatusCodeResult(StatusCodes.Status415UnsupportedMediaType));
        }

        T? model;
        try
        {
            model = await request.ReadFromJsonAsync<T>(SerializerOptions, cancellationToken);
        }
        catch (JsonException ex)
        {
            return Invalid<T>(ex.Path ?? "$", ex.Message);
        }

        if (model is null)
        {
            return Invalid<T>(string.Empty, "A non-empty request body is required.");
        }

        var results = new List<ValidationResult>();
        if (Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true))
        {
            return new BodyResult<T>(model, null);
        }

        var errors = results
            .SelectMany(r => (r.MemberNames.Any() ? r.MemberNames : [string.Empty])
                .Select(member => (member, message: r.ErrorMessage ?? "Invalid value.")))
            .GroupBy(e => e.member)
            .ToDictionary(g => g.Key, g => g.Select(e => e.message).ToArray());

        return new BodyResult<T>(null, ValidationProblem(errors));
    }

    private static BodyResult<T> Invalid<T>(string key, string message)
        where T : class =>
        new(null, ValidationProblem(new Dictionary<string, string[]> { [key] = [message] }));

    private static BadRequestObjectResult ValidationProblem(IDictionary<string, string[]> errors)
    {
        var problem = new ValidationProblemDetails(errors)
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
        };

        var result = new BadRequestObjectResult(problem);
        result.ContentTypes.Add("application/problem+json");
        return result;
    }
}

internal readonly record struct BodyResult<T>(T? Model, IActionResult? Error)
    where T : class;
