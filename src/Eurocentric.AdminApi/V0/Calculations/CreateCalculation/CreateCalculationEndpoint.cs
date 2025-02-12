using Eurocentric.AdminApi.V0.Calculations.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

public static class CreateCalculationEndpoint
{
    public static async Task<Ok<CreateCalculationResponse>> ExecuteAsync(
        [FromBody] CreateCalculationRequest request,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine(request);

        CreateCalculationResponse result = await sender.Send(request.ToCommand(), cancellationToken);

        return TypedResults.Ok(result);
    }

    public static void MapCreateCalculationEndpoint(this IEndpointRouteBuilder app) => app
        .MapPost("calculations", ExecuteAsync)
        .WithName(nameof(CreateCalculation))
        .WithSummary("Create calculation")
        .WithTags("Calculations");

    internal sealed class OperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context,
            CancellationToken cancellationToken)
        {
            if (operation.OperationId.EndsWith(nameof(CreateCalculation)))
            {
                operation.Responses["200"] = new OpenApiResponse
                {
                    Description = "Created calculation",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new()
                        {
                            Examples = new Dictionary<string, OpenApiExample>
                            {
                                ["product of 2 and 5"] = new()
                                {
                                    Description = "product of 2 and 5",
                                    Value = new OpenApiObject
                                    {
                                        ["calculation"] = new OpenApiObject
                                        {
                                            ["x"] = new OpenApiInteger(2),
                                            ["y"] = new OpenApiInteger(5),
                                            ["operation"] =
                                                new OpenApiString(Operation.Product.ToString()),
                                            ["result"] = new OpenApiInteger(10)
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
