using ErrorOr;
using Eurocentric.AdminApi.V0.Calculations.GetCalculation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal static class CreateCalculationEndpoint
{
    internal static void MapCreateCalculationV0Point2(this IEndpointRouteBuilder app) =>
        app.MapPost("admin/api/v0.2/calculations", async ([FromBody] CreateCalculationCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            ErrorOr<CreateCalculationResult> errorsOrResult = await sender.Send(command, cancellationToken);

            return TypedResults.CreatedAtRoute(errorsOrResult.Value,
                nameof(GetCalculationEndpoint),
                new RouteValueDictionary { ["calculationId"] = errorsOrResult.Value.Calculation.Id });
        });
}
