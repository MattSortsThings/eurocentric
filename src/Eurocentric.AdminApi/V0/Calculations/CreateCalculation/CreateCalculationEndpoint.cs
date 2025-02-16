using ErrorOr;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal static class CreateCalculationEndpoint
{
    internal static void MapCreateCalculation(this IEndpointRouteBuilder api) => api.MapPost("admin/api/v0.1/calculations",
            async (CreateCalculationCommand command, ISender sender, CancellationToken cancellationToken = default) =>
            {
                ErrorOr<CreateCalculationResult> result = await sender.Send(command, cancellationToken);

                return result.ToHttpResult(TypedResults.Ok);
            })
        .WithSummary("Create calculation")
        .WithTags("Calculations")
        .Produces<CreateCalculationResult>();
}
