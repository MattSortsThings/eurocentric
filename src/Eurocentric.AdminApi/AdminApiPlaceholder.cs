using Eurocentric.AdminApi.V0.Contests.CreateContest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi;

public static class AdminApiPlaceholder
{
    public static IEndpointRouteBuilder MapAdminApiPlaceholderEndpoint(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("admin/api/v0.1")
            .AllowAnonymous();

        api.MapCreateContest();

        return app;
    }
}
