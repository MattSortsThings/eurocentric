using Eurocentric.Features.PublicApi.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Public API endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    internal static void MapPublicApiEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.NewVersionedApi("PublicApi")
            .MapGroup("public/api");

        apiGroup.MapV0Endpoints();
    }
}
