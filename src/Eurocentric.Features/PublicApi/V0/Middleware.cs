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
    ///     Maps the versioned endpoints for version 0 of the Public API.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapPublicApiV0Endpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("PublicApi.V0")
            .MapGroup("public/api/v{version:apiVersion}")
            .WithGroupName("PublicApi.V0")
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithUserRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        apiGroup.MapGetAvailableContestStages()
            .MapGetAvailableVotingMethods();

        apiGroup.MapGetPointsShareVotingCountryRankings();
    }
}
