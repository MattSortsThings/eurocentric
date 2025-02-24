using Asp.Versioning;
using ErrorOr;
using Eurocentric.PublicApi.Common;
using Eurocentric.Shared.ApiRegistration;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.PublicApi.V0.Stations.GetStations;

internal sealed record GetStationsEndpoint : IEndpointInfo
{
    private const string Name = "GetStations";

    public Delegate Handler => async ([AsParameters] GetStationsQuery query,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetStationsResult> errorsOrResult = await sender.Send(query, cancellationToken);

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public string Resource => "stations";

    public HttpMethod Method => HttpMethod.Get;

    public string EndpointId => Name;

    public ApiVersion InitialApiVersion => ApiVersions.V0.Point1;

    public string Tag => ApiTags.Stations;

    public string Summary => "Get stations";

    public string Description => "Retrieves a list of stations matching the query.";

    public IEnumerable<int> ProblemStatusCodes =>
        PublicApiInfo.UniversalProblemStatusCodes.Append(StatusCodes.Status400BadRequest);
}
