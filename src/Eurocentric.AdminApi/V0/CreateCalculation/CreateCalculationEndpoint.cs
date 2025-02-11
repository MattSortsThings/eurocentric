using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.CreateCalculation;

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

    public static void MapAdminApiPlaceholderEndpoint(this IEndpointRouteBuilder app) => app
        .MapPost("admin/api/v0.1/calculations", ExecuteAsync)
        .WithSummary("Create calculation")
        .WithTags("Calculations");
}
