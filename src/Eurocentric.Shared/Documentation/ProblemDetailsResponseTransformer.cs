using Eurocentric.Shared.ErrorHandling;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Shared.Documentation;

internal sealed class ProblemDetailsResponseTransformer : IOpenApiOperationTransformer
{
    private readonly IOpenApiAny _traceIdExample = new OpenApiString("00-82116fe99e884d7f351e20a84e337da3-162fb1936f055f6e-00");

    public Task TransformAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        foreach ((string statusCode, OpenApiResponse response) in operation.Responses)
        {
            if (statusCode.StartsWith('2'))
            {
                continue;
            }

            string title = GetProblemDetailsTitle(statusCode);

            response.Content["application/problem+json"].Examples[title] = new OpenApiExample
            {
                Value = new OpenApiObject
                {
                    ["type"] = new OpenApiString(GetProblemDetailsType(statusCode)),
                    ["title"] = new OpenApiString(title),
                    ["status"] = new OpenApiInteger(int.Parse(statusCode)),
                    ["instance"] = new OpenApiString(context.Description.GetInstance()),
                    ["traceId"] = _traceIdExample
                }
            };
        }

        return Task.CompletedTask;
    }

    private static string GetProblemDetailsType(string statusCode) => statusCode switch
    {
        "400" => StatusCodeUrls.Status400BadRequest,
        "401" => StatusCodeUrls.Status401Unauthorized,
        "403" => StatusCodeUrls.Status403Forbidden,
        "404" => StatusCodeUrls.Status404NotFound,
        "409" => StatusCodeUrls.Status409Conflict,
        "422" => StatusCodeUrls.Status422UnprocessableEntity,
        "500" => StatusCodeUrls.Status500InternalServerError,
        _ => throw new InvalidOperationException($"Unsupported status code: {statusCode}.")
    };

    private static string GetProblemDetailsTitle(string statusCode) => statusCode switch
    {
        "400" => "Bad request",
        "401" => "Unauthorized",
        "403" => "Forbidden",
        "404" => "Not found",
        "409" => "Conflict",
        "422" => "Unprocessable entity",
        "500" => "Internal server error",
        _ => throw new InvalidOperationException($"Unsupported status code: {statusCode}.")
    };
}
