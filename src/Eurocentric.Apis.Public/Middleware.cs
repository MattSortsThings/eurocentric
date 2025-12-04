using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware
/// </summary>
public static class Middleware
{
    public static void UsePlaceholderPublicApiEndpoint(this IEndpointRouteBuilder builder) =>
        builder.MapGet("public/api/ping", () => TypedResults.Ok("Public API zapped to the extreme!"));
}
