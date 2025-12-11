using Eurocentric.Apis.Public.V0;
using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Maps all the endpoints defined in the <see cref="Eurocentric.Apis.Public" /> namespace.
/// </summary>
public sealed class PublicApiEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder publicApiGroup = routeBuilder.MapGroup("public/api");

        publicApiGroup.Map<V0Endpoints>();
    }
}
