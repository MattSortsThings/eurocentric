using Asp.Versioning;
using ErrorOr;
using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Calculations.GetCalculation;

internal sealed class GetCalculationEndpoint : IApiEndpoint
{
    private static Delegate Handler => async ([FromRoute(Name = "calculationId")] Guid calculationId,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetCalculationResult> errorsOrResult =
            await sender.Send(new GetCalculationQuery(calculationId), cancellationToken);

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public string EndpointName => nameof(GetCalculation);

    public ApiVersion InitialApiVersion => new(0, 1);

    public RouteHandlerBuilder Map(IEndpointRouteBuilder apiGroup) =>
        apiGroup.MapGet("calculations/{calculationId:guid}", Handler)
            .WithSummary("Get calculation")
            .WithTags("Calculations")
            .Produces<GetCalculationResult>();

    public void Configure(OpenApiOptions openApiOptions) { }
}
