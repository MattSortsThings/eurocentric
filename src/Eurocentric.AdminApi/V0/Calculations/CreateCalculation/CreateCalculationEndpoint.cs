using ErrorOr;
using Eurocentric.Shared.ApiAbstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal sealed record CreateCalculationEndpoint : IEndpointInfo
{
    public string Name => nameof(CreateCalculation);

    public Delegate Handler => async ([FromBody] CreateCalculationCommand command,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<CreateCalculationResult> errorsOrResult = await sender.Send(command, cancellationToken);

        return MapToCreatedAtRoute(errorsOrResult.Value);
    };

    public HttpMethod Method => HttpMethod.Post;

    public string Route => "calculations";

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 2;

    private static CreatedAtRoute<CreateCalculationResult> MapToCreatedAtRoute(CreateCalculationResult result) =>
        TypedResults.CreatedAtRoute(result,
            nameof(GetCalculation),
            new RouteValueDictionary { ["calculationId"] = result.Calculation.Id });
}
