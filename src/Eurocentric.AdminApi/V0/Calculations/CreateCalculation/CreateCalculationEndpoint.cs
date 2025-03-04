using ErrorOr;
using Eurocentric.AdminApi.Common;
using Eurocentric.AdminApi.V0.Calculations.Models;
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

    public string Summary => "Create a calculation";

    public string Description => "Creates a new calculation. " +
                                 "The client must supply the calculation parameters in the request body.";

    public string Tag => EndpointTags.Calculations;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status422UnprocessableEntity;
        }
    }

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new CreateCalculationCommand { X = 10, Y = 4, Operation = Operation.Modulus };
            yield return new CreateCalculationResult(new Calculation(Guid.NewGuid(), 10, 4, Operation.Modulus, 2));
        }
    }

    private static CreatedAtRoute<CreateCalculationResult> MapToCreatedAtRoute(CreateCalculationResult result) =>
        TypedResults.CreatedAtRoute(result,
            nameof(GetCalculation),
            new RouteValueDictionary { ["calculationId"] = result.Calculation.Id });
}
