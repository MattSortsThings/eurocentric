using Eurocentric.Apis.Admin.V0;
using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Maps all the endpoints defined in the <see cref="Eurocentric.Apis.Admin" /> namespace.
/// </summary>
public sealed class AdminApiEndpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder adminApiGroup = routeBuilder.MapGroup("admin/api");

        adminApiGroup.Map<V0Endpoints>();
    }
}
