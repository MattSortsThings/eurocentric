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
            .MapGroup("/")
            .WithGroupName(EndpointConstants.GroupName)
            .AllowAnonymous();

        v0Group
            .Map<GetQueryableBroadcastsV0Point1.EndpointMapper>()
            .Map<GetQueryableCountriesV0Point1.EndpointMapper>();

        v0Group.Map<GetBroadcastResultListingsV0Point2.EndpointMapper>();

        v0Group.Map<GetCompetingCountryPointsAverageRankingsV0Point2.EndpointMapper>();

        v0Group
            .Map<GetQueryableBroadcastsV0Point2.EndpointMapper>()
            .Map<GetQueryableContestsV0Point2.EndpointMapper>()
            .Map<GetQueryableCountriesV0Point2.EndpointMapper>();
    }
}
