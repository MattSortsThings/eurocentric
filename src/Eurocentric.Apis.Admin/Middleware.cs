using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware
/// </summary>
public static class Middleware
{
    public static void UsePlaceholderAdminApiEndpoint(this IEndpointRouteBuilder builder) =>
        builder.MapGet("admin/api/ping", () => TypedResults.Ok("Admin API zapped to the extreme!"));
}
