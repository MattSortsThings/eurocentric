using Asp.Versioning;
using ErrorOr;
using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V0.Stations.GetStations;

internal sealed class GetStationsEndpoint : IApiEndpoint
{
    private static Delegate Handler => async ([AsParameters] GetStationsQuery query,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetStationsResult> errorsOrResult = await sender.Send(query, cancellationToken);

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public string EndpointName => nameof(GetStations);

    public ApiVersion InitialApiVersion => new(0, 1);

    public RouteHandlerBuilder Map(IEndpointRouteBuilder apiGroup) =>
        apiGroup.MapGet("stations", Handler)
            .WithSummary("Get stations")
            .WithTags("Stations")
            .Produces<GetStationsResult>();
}
