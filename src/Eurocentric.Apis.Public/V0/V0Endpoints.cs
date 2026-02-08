using Eurocentric.Apis.Public.V0.Common.Constants;
using Eurocentric.Apis.Public.V0.QueryableBroadcasts;
using Eurocentric.Apis.Public.V0.QueryableContests;
using Eurocentric.Apis.Public.V0.QueryableCountries;
using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder.MapGroup("/").WithGroupName(EndpointGroup.Name);

        v0Group
            .Map<GetQueryableBroadcastsV0Point1.Endpoint>()
            .Map<GetQueryableContestsV0Point1.Endpoint>()
            .Map<GetQueryableCountriesV0Point1.Endpoint>();
    }
}
