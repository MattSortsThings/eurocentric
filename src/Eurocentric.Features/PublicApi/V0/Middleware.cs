using Eurocentric.Features.PublicApi.V0.Filters;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V0;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Public API V0 endpoints.
    /// </summary>
    /// <param name="publicApiGroup">The Public API endpoint group.</param>
    internal static void MapPublicApiV0Endpoints(this IEndpointRouteBuilder publicApiGroup)
    {
        RouteGroupBuilder v0Group = publicApiGroup.MapGroup("v{version:apiVersion}")
            .WithGroupName("PublicApi.V0")
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithUserRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v0Group.MapGetAvailableVotingMethods()
            .MapGetPointsShareVotingCountryRankings();
    }
}
