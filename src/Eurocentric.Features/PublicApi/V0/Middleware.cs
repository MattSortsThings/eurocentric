using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Filters;
using Eurocentric.Features.PublicApi.V0.Rankings;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V0;

/// <summary>
///     Extension methods to invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Public API version 0 endpoints to the Public API endpoint group.
    /// </summary>
    /// <param name="apiGroup">The Public API endpoint group.</param>
    internal static void MapPublicApiV0Endpoints(this IEndpointRouteBuilder apiGroup)
    {
        RouteGroupBuilder v0Group = apiGroup.MapGroup("v{apiVersion:apiVersion}")
            .RequireAuthorization(nameof(AuthorizationPolicies.RequireAuthenticatedClientWithUserRole))
            .WithGroupName(ApiNames.EndpointGroup)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v0Group.MapGetContestStages()
            .MapGetCountries()
            .MapGetVotingMethods();

        v0Group.MapGetCompetingCountryPointsAverageRankings()
            .MapGetCompetingCountryPointsInRangeRankings();
    }
}
