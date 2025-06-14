using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the versioned endpoints for version 1 of the Admin API.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapAdminApiV1Endpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiReleases.Id)
            .MapGroup("admin/api/v{version:apiVersion}")
            .WithGroupName(ApiReleases.EndpointGroupName)
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        apiGroup.MapGetBroadcast()
            .MapGetBroadcasts()
            .MapAwardJuryPoints()
            .MapAwardTelevotePoints()
            .MapDisqualifyCompetitor()
            .MapDeleteBroadcast();

        apiGroup.MapGetContest()
            .MapGetContests()
            .MapCreateContest()
            .MapCreateChildBroadcast()
            .MapDeleteContest();

        apiGroup.MapGetCountry()
            .MapGetCountries()
            .MapCreateCountry()
            .MapDeleteCountry();
    }
}
