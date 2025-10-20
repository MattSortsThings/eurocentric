using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Features.Listings;
using Eurocentric.Apis.Public.V0.Features.Queryables;
using Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

internal static class V0EndpointGroup
{
    internal static void MapV0EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v0Group = routeBuilder
            .MapGroup("v{version:apiVersion}")
            .WithGroupName(EndpointConstants.GroupName)
            .AllowAnonymous();

        v0Group.Map<GetCompetingCountryPointsAverageRankings.EndpointMapper>();

        v0Group.Map<GetBroadcastResultListings.EndpointMapper>();

        v0Group
            .Map<GetQueryableBroadcasts.EndpointMapper>()
            .Map<GetQueryableContests.EndpointMapper>()
            .Map<GetQueryableCountries.EndpointMapper>();
    }
}
