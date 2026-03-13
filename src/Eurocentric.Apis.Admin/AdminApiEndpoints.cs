using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Endpoint mapper for the Admin API.
/// </summary>
public sealed class AdminApiEndpoints : IEndpointMapper
{
    /// <inheritdoc />
    /// <summary>Maps the Admin API endpoints to the provided endpoint route builder.</summary>
    public static void MapTo(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("admin/api").AllowAnonymous();

        apiGroup
            .MapGet("blobby", () => TypedResults.Ok("Admin API says: Blobby Blobby Blobby!"))
            .WithSummary("Get Blobby")
            .WithName("AdminApi.V0.GetBlobby")
            .WithTags("Admin Placeholders");
    }
}
