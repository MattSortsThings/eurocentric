using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.CompetingCountryRankings;
using Eurocentric.Features.PublicApi.V0.Queryables;
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
    ///     Maps the Public API v0.x endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    internal static void MapV0Endpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder.MapGroup("v{version:apiVersion}")
            .WithGroupName(Endpoints.Group)
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithUserRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v0Group.MapCompetingCountryRankingsEndpoints();
        v0Group.MapQueryablesEndpoints();
    }
}
