using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Eurocentric.Components.OpenApi;

public sealed class ProblemDetailsResponseExampleTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        string instance = $"{context.Description.HttpMethod} {context.Description.RelativePath}";

        foreach ((int statusCode, OpenApiMediaType type) in GetProblemDetailsResponses(operation.Responses))
        {
            string title = MapToExampleTitle(statusCode);
            type.Examples = new Dictionary<string, IOpenApiExample>
            {
                [title] = new OpenApiExample
                {
                    Value = new JsonObject
                    {
                        ["type"] = MapToExampleType(statusCode),
                        ["title"] = title,
                        ["status"] = statusCode,
                        ["detail"] = MapToExampleDetail(statusCode),
                        ["instance"] = instance,
                    },
                },
            };
        }

        return Task.CompletedTask;
    }

    private static IEnumerable<(int StatusCode, OpenApiMediaType Type)> GetProblemDetailsResponses(
        OpenApiResponses? responses
    )
    {
        foreach ((string statusCode, IOpenApiResponse response) in responses ?? [])
        {
            if (
                response.Content is { } content
                && content.TryGetValue("application/problem+json", out OpenApiMediaType? type)
            )
            {
                yield return (int.Parse(statusCode), type);
            }
        }
    }

    private static string MapToExampleTitle(int statusCode) =>
        statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            422 => "Unprocessable Entity",
            _ => throw new NotSupportedException($"No example title defined for status code {statusCode}."),
        };

    private static string MapToExampleDetail(int statusCode) =>
        statusCode switch
        {
            400 => "Request could not be understood.",
            401 => "Client is not authenticated.",
            403 => "Client is not authorized to access resource.",
            404 => "Requested resource was not found.",
            409 => "Request conflicts with the current state of the system.",
            422 => "Request contains one or more illegal values.",
            _ => throw new NotSupportedException($"No example detail defined for status code {statusCode}."),
        };

    private static string MapToExampleType(int statusCode) =>
        statusCode switch
        {
            400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            422 => "https://tools.ietf.org/html/rfc9110#section-15.5.21",
            _ => throw new NotSupportedException($"No example type defined for status code {statusCode}."),
        };
}
