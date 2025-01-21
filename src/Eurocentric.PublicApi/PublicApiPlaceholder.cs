using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi;

public static class PublicApiPlaceholder
{
    public static IEndpointRouteBuilder MapPublicApiPlaceholderEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("public/api/v1.0/placeholder", () => "Hello world from Public API V1.0!")
            .AllowAnonymous();

        return app;
    }
}
