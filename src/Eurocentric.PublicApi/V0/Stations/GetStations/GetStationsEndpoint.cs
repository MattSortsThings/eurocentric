using Asp.Versioning;
using ErrorOr;
using Eurocentric.PublicApi.Common;
using Eurocentric.PublicApi.V0.Stations.Models;
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

    public ApiVersion InitialApiVersion => PublicApiInfo.Versions.V0.Point1;

    public string Tag => PublicApiInfo.Tags.Stations;

    public string Summary => "Get stations";

    public string Description => "Retrieves a list of stations matching the query.";

    public IEnumerable<int> ProblemStatusCodes =>
        PublicApiInfo.UniversalProblemStatusCodes.Append(StatusCodes.Status400BadRequest);

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new GetStationsResult([
                new Station("Green Park", Line.Jubilee),
                new Station("Westminster", Line.Jubilee),
                new Station("Canada Water", Line.Jubilee),
                new Station("Canary Wharf", Line.Jubilee),
                new Station("North Greenwich", Line.Jubilee)
            ]);
        }
    }
}
