using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal sealed class ProblemDetailsResponseExampleOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        string instance = $"{context.Description.HttpMethod} {context.Description.RelativePath}";

        foreach ((string statusCode, OpenApiResponse response) in operation.Responses)
        {
            if (response.Content.TryGetValue("application/problem+json", out OpenApiMediaType? content))
            {
                AddProblemDetailsExample(statusCode, content, instance);
            }
        }

        return Task.CompletedTask;
    }

    private static void AddProblemDetailsExample(string statusCode, OpenApiMediaType content, string instance)
    {
        string title = CreateExampleTitle(statusCode);

        content.Examples.Add(title,
            new OpenApiExample
            {
                Value = new OpenApiObject
                {
                    ["type"] = new OpenApiString(CreateExampleType(statusCode)),
                    ["title"] = new OpenApiString(title),
                    ["status"] = new OpenApiInteger(int.Parse(statusCode)),
                    ["detail"] = new OpenApiString(CreateExampleDetail(statusCode)),
                    ["instance"] = new OpenApiString(instance)
                }
            });
    }

    private static string CreateExampleTitle(string statusCode) => statusCode switch
    {
        "400" => "Bad Request",
        "401" => "Unauthorized",
        "403" => "Forbidden",
        "404" => "Not Found",
        "409" => "Conflict",
        "422" => "Unprocessable Entity",
        _ => throw new NotSupportedException($"No example problem details title defined for status code {statusCode}.")
    };

    private static string CreateExampleDetail(string statusCode) => statusCode switch
    {
        "400" => "Request could not be understood.",
        "401" => "Client is not authenticated.",
        "403" => "Client is not authorized to access resource.",
        "404" => "Requested resource was not found.",
        "409" => "Request conflicts with the current state of the system.",
        "422" => "Request contains one or more illegal values.",
        _ => throw new NotSupportedException($"No example problem details title defined for status code {statusCode}.")
    };

    private static string CreateExampleType(string statusCode) => statusCode switch
    {
        "400" => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        "401" => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
        "403" => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
        "404" => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        "409" => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        "422" => "https://tools.ietf.org/html/rfc4918#section-11.2",
        _ => throw new NotSupportedException($"No example problem details title defined for status code {statusCode}.")
    };
}
