using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Calculations.GetCalculation;

internal static class GetCalculationEndpoint
{
    internal static void MapGetCalculationV0Point1(this IEndpointRouteBuilder app) => app.MapGet(
        "admin/api/v0.1/calculations/{calculationId:guid}",
        async (Guid calculationId,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            ErrorOr<GetCalculationResult> errorsOrResult =
                await sender.Send(new GetCalculationQuery(calculationId), cancellationToken);

            return TypedResults.Ok(errorsOrResult.Value);
        });

    internal static void MapGetCalculationV0Point2(this IEndpointRouteBuilder app) => app.MapGet(
            "admin/api/v0.2/calculations/{calculationId:guid}",
            async (Guid calculationId,
                ISender sender,
                CancellationToken cancellationToken = default) =>
            {
                ErrorOr<GetCalculationResult> errorsOrResult =
                    await sender.Send(new GetCalculationQuery(calculationId), cancellationToken);

                return TypedResults.Ok(errorsOrResult.Value);
            })
        .WithName(nameof(GetCalculationEndpoint));
}
