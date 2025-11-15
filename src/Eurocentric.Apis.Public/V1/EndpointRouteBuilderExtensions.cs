using Eurocentric.Apis.Public.V1.Config;
using Eurocentric.Apis.Public.V1.Features.CompetingCountryRankings;
using Eurocentric.Apis.Public.V1.Features.CompetitorRankings;
using Eurocentric.Apis.Public.V1.Features.Listings;
using Eurocentric.Apis.Public.V1.Features.Queryables;
using Eurocentric.Apis.Public.V1.Features.VotingCountryRankings;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V1;

internal static class EndpointRouteBuilderExtensions
{
    internal static void MapV1EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v1Group = routeBuilder
            .MapGroup("v{version:apiVersion}")
            .WithGroupName(V1Group.Name)
            .RequiresAuthenticatedClient()
            .RequiresUserRole()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v1Group
            .Map<GetCompetingCountryPointsAverageRankings.EndpointMapper>()
            .Map<GetCompetingCountryPointsConsensusRankings.EndpointMapper>()
            .Map<GetCompetingCountryPointsInRangeRankings.EndpointMapper>()
            .Map<GetCompetingCountryPointsShareRankings.EndpointMapper>();

        v1Group
            .Map<GetCompetitorPointsAverageRankings.EndpointMapper>()
            .Map<GetCompetitorPointsConsensusRankings.EndpointMapper>()
            .Map<GetCompetitorPointsInRangeRankings.EndpointMapper>()
            .Map<GetCompetitorPointsShareRankings.EndpointMapper>();

        v1Group.Map<GetCompetingCountryPointsListings.EndpointMapper>();

        v1Group
            .Map<GetQueryableBroadcasts.EndpointMapper>()
            .Map<GetQueryableContests.EndpointMapper>()
            .Map<GetQueryableCountries.EndpointMapper>();

        v1Group
            .Map<GetVotingCountryPointsAverageRankings.EndpointMapper>()
            .Map<GetVotingCountryPointsConsensusRankings.EndpointMapper>()
            .Map<GetVotingCountryPointsInRangeRankings.EndpointMapper>()
            .Map<GetVotingCountryPointsShareRankings.EndpointMapper>();
    }
}
