using ErrorOr;
using Eurocentric.Shared.ApiAbstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.AdminApi.V0.Calculations.GetCalculation;

public sealed record GetCalculationEndpoint : IEndpointInfo
{
    public string Name => nameof(GetCalculation);

    public Delegate Handler => async ([FromRoute] Guid calculationId,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetCalculationResult> errorsOrResult = await sender.Send(MapToQuery(calculationId), cancellationToken);

        return TypedResults.Ok(errorsOrResult.Value);
    };

    public HttpMethod Method => HttpMethod.Get;

    public string Route => "calculations/{calculationId:guid}";

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 1;

    private static GetCalculationQuery MapToQuery(Guid calculationId) => new(calculationId);
}
