using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Endpoint mapper for the Public API.
/// </summary>
public sealed class PublicApiEndpoints : IEndpointMapper
{
    /// <inheritdoc />
    /// <summary>Maps the Public API endpoints to the provided endpoint route builder.</summary>
    public static void MapTo(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("public/api").AllowAnonymous();

        apiGroup
            .MapGet("blobby", () => TypedResults.Ok("Public API says: Blobby Blobby Blobby!"))
            .WithSummary("Get Blobby")
            .WithName("PublicApi.V0.GetBlobby")
            .WithTags("Public Placeholders");
    }
}
