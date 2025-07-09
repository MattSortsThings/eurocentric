using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Contests;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V0;

/// <summary>
///     Extension methods to invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Maps the Admin API version 0 endpoints to the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void MapAdminApiV0Endpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiNames.Id)
            .MapGroup("admin/api/v{apiVersion:apiVersion}")
            .RequireAuthorization(nameof(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole))
            .WithGroupName(ApiNames.EndpointGroup);

        apiGroup.MapCreateContest()
            .MapGetContest()
            .MapGetContests();
    }
}
