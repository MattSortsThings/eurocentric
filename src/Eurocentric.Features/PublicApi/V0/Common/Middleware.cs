using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V0.Common;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the Public API v0.x endpoints.
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapV0Endpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder.MapGroup("v{version:apiVersion}")
            .WithGroupName(EndpointNames.Group)
            .AllowAnonymous()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v0Group.MapGet("placeholder", () => TypedResults.Ok($"Public API zapped to the extreme at {DateTime.Now}!"))
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2);
    }
}
