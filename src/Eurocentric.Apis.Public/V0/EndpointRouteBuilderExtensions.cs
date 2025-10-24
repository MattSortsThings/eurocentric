using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Features.Listings;
using Eurocentric.Apis.Public.V0.Features.Queryables;
using Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

internal static class EndpointRouteBuilderExtensions
{
    internal static void MapV0EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v0Group = routeBuilder
            .MapGroup("v{version:apiVersion}")
            .WithGroupName(V0Group.Name)
            .RequiresAuthenticatedClient()
            .RequiresUserRole()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v0Group.Map<GetCompetingCountryPointsAverageRankings.EndpointMapper>();

        v0Group.Map<GetBroadcastResultListings.EndpointMapper>();

        v0Group
            .Map<GetQueryableBroadcasts.EndpointMapper>()
            .Map<GetQueryableContests.EndpointMapper>()
            .Map<GetQueryableCountries.EndpointMapper>();
    }
}
