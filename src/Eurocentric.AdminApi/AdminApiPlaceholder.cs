using Eurocentric.AdminApi.V1.Contests.CreateContest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi;

public static class AdminApiPlaceholder
{
    public static IEndpointRouteBuilder MapAdminApiPlaceholderEndpoint(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("admin/api/v1.0")
            .AllowAnonymous();

        api.MapCreateContest();

        return app;
    }
}
