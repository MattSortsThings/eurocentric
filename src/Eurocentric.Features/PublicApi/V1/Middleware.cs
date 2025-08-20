using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Queryables;
using Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries;
using Eurocentric.Features.PublicApi.V1.Rankings.Competitors;
using Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V1;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the Public API v1.x endpoints.
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapV1Endpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v1Group = builder.MapGroup("v{version:apiVersion}")
            .WithGroupName(EndpointNames.Group)
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithUserRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v1Group.MapCompetingCountryRankingsEndpoints();
        v1Group.MapCompetitorRankingsEndpoints();
        v1Group.MapQueryablesEndpoints();
        v1Group.MapVotingCountryRankingsEndpoints();
    }
}
