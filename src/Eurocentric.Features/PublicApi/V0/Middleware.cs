using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Filters;
using Eurocentric.Features.PublicApi.V0.Rankings;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V0;

/// <summary>
///     Extension methods to invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Maps the Public API version 0 endpoints to the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void MapPublicApiV0Endpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiNames.Id)
            .MapGroup("public/api/v{apiVersion:apiVersion}")
            .RequireAuthorization(nameof(AuthorizationPolicies.RequireAuthenticatedClient))
            .WithGroupName(ApiNames.EndpointGroup);

        apiGroup.MapGetContestStages()
            .MapGetCountries()
            .MapGetVotingMethods();

        apiGroup.MapGetCompetingCountryPointsAverageRankings()
            .MapGetCompetingCountryPointsInRangeRankings();
    }
}
