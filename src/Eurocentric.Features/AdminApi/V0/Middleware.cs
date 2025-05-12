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
    ///     Maps the Admin API V0 endpoints.
    /// </summary>
    /// <param name="adminApiGroup">The Admin API endpoint group.</param>
    internal static void MapAdminApiV0Endpoints(this IEndpointRouteBuilder adminApiGroup)
    {
        RouteGroupBuilder v0Group = adminApiGroup.MapGroup("v{version:apiVersion}")
            .WithGroupName("AdminApi.V0")
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        v0Group.MapGetContest()
            .MapGetContests()
            .MapCreateContest();
    }
}
