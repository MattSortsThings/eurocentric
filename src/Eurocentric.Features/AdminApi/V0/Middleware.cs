using Eurocentric.Features.AdminApi.V0.Contests;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V0;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the versioned endpoints for version 0 of the Admin API.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapAdminApiV0Endpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("AdminApi.V0")
            .MapGroup("admin/api/v{version:apiVersion}")
            .WithGroupName("AdminApi.V0")
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        apiGroup.MapGetContest()
            .MapGetContests()
            .MapCreateContest();
    }
}
