using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Contests;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V0;

/// <summary>
///     Extension methods to be invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Admin API version 0 endpoints to the Admin API endpoint group.
    /// </summary>
    /// <param name="apiGroup">The Admin API endpoint group.</param>
    internal static void MapAdminApiV0Endpoints(this IEndpointRouteBuilder apiGroup)
    {
        RouteGroupBuilder v0Group = apiGroup.MapGroup("v{apiVersion:apiVersion}")
            .RequireAuthorization(nameof(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole))
            .WithGroupName(ApiNames.EndpointGroup)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        v0Group.MapCreateContest()
            .MapGetContest()
            .MapGetContests();
    }
}
