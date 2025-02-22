using Asp.Versioning;
using ErrorOr;
using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal sealed class CreateCalculationEndpoint : IApiEndpoint
{
    private static Delegate Handler => async ([FromBody] CreateCalculationCommand command,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<CreateCalculationResult> errorsOrResult = await sender.Send(command, cancellationToken);

        return errorsOrResult.ToHttpResult(MapToCreatedAtRoute);
    };

    public string EndpointName => nameof(CreateCalculation);

    public ApiVersion InitialApiVersion => new(0, 2);

    public RouteHandlerBuilder Map(IEndpointRouteBuilder apiGroup) =>
        apiGroup.MapPost("calculations", Handler)
            .WithSummary("Create calculation")
            .WithTags("Calculations")
            .Produces<CreateCalculationResult>(StatusCodes.Status201Created);

    private static IResult MapToCreatedAtRoute(CreateCalculationResult result) => TypedResults.CreatedAtRoute(result,
        nameof(GetCalculation),
        new RouteValueDictionary { ["calculationId"] = result.Calculation.Id });
}
