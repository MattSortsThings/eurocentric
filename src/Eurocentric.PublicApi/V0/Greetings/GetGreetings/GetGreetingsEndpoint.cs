using Eurocentric.PublicApi.V0.Greetings.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

public static class GetGreetingsEndpoint
{
    public static async Task<Ok<GetGreetingsResponse>> ExecuteAsync([AsParameters] GetGreetingsRequest request,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        GetGreetingsResponse result = await sender.Send(request.ToQuery(), cancellationToken);

        return TypedResults.Ok(result);
    }

    public static void MapGetGreetings(this IEndpointRouteBuilder app) => app
        .MapGet("greetings", ExecuteAsync)
        .WithName(nameof(GetGreetings))
        .WithSummary("Get greetings")
        .WithDescription("Retrieves an array of identical greetings from the query parameters.")
        .WithTags("Greetings");

    internal sealed class OperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context,
            CancellationToken cancellationToken)
        {
            if (operation.OperationId.EndsWith(nameof(GetGreetings)))
            {
                operation.Responses["200"] = new OpenApiResponse
                {
                    Description = "Generated greetings",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new()
                        {
                            Examples = new Dictionary<string, OpenApiExample>
                            {
                                ["1 English greeting"] =
                                    new()
                                    {
                                        Description = "1 English greeting",
                                        Value = new OpenApiObject
                                        {
                                            ["greetings"] = new OpenApiArray
                                            {
                                                new OpenApiObject
                                                {
                                                    ["message"] = new OpenApiString("hi!"),
                                                    ["language"] = new OpenApiString(Language.English.ToString())
                                                }
                                            }
                                        }
                                    },
                                ["2 Dutch greetings"] = new()
                                {
                                    Description = "2 Dutch greetings",
                                    Value = new OpenApiObject
                                    {
                                        ["greetings"] = new OpenApiArray
                                        {
                                            new OpenApiObject
                                            {
                                                ["message"] = new OpenApiString("hoi!"),
                                                ["language"] = new OpenApiString(Language.Dutch.ToString())
                                            },
                                            new OpenApiObject
                                            {
                                                ["message"] = new OpenApiString("hoi!"),
                                                ["language"] = new OpenApiString(Language.Dutch.ToString())
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }

            return Task.CompletedTask;
        }
    }
}
