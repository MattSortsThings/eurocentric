using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi;

public static class AdminApiPlaceholder
{
    public static IEndpointRouteBuilder MapAdminApiPlaceholderEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/api/v1.0/placeholder", () => "Hello world from Admin API V1.0!")
            .AllowAnonymous();

        return app;
    }
}
