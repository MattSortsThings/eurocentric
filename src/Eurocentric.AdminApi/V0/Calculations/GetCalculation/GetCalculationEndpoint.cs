using ErrorOr;
using Eurocentric.AdminApi.Common;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.Shared.ApiAbstractions;
using Eurocentric.Shared.ErrorHandling;
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

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public HttpMethod Method => HttpMethod.Get;

    public string Route => "calculations/{calculationId:guid}";

    public int MajorApiVersion => 0;

    public int MinorApiVersion => 1;

    public string Summary => "Get a calculation";

    public string Description => "Retrieves a single calculation." +
                                 " The client must supply the calculation ID as a route parameter.";

    public string Tag => EndpointTags.Calculations;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status404NotFound;
        }
    }

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new GetCalculationResult(new Calculation(Guid.NewGuid(), 10, 4, Operation.Modulus, 2));
        }
    }

    private static GetCalculationQuery MapToQuery(Guid calculationId) => new(calculationId);
}
