using Eurocentric.Features.AdminApi.V0.Contests;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("AdminApi")
            .MapGroup("admin/api/v{version:apiVersion}")
            .WithGroupName("AdminApi")
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        apiGroup.MapGetContest()
            .MapGetContests()
            .MapCreateContest();
    }
}
