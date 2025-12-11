using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("v0.1/ping", () => TypedResults.Ok("Admin API v0.1 zapped to the extreme!"))
            .WithTags("Placeholders");

        routeBuilder
            .MapGet("v0.2/ping", () => TypedResults.Ok("Admin API v0.2 zapped to the extreme!"))
            .WithTags("Placeholders");
    }
}
