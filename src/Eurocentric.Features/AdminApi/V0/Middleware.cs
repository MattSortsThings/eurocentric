using Eurocentric.Features.AdminApi.V0.Contests;
using Microsoft.AspNetCore.Builder;
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
        RouteGroupBuilder apiGroup = app.MapGroup("admin/api")
            .WithGroupName("AdminApi.V0")
            .AllowAnonymous();

        apiGroup.MapGetContest()
            .MapGetContests()
            .MapCreateContest();
    }
}
