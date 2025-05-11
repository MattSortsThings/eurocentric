using Eurocentric.Features.PublicApi.V0.Filters;
using Eurocentric.Features.PublicApi.V0.VotingCountryRankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Public API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapPublicApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.MapGroup("public/api")
            .WithGroupName("PublicApi")
            .AllowAnonymous();

        apiGroup.MapGetAvailableVotingMethods()
            .MapGetPointsShareVotingCountryRankings();
    }
}
