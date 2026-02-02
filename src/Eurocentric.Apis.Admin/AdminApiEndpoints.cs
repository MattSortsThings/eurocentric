using Eurocentric.Apis.Admin.V0;
using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Maps all endpoints defined in the <see cref="Eurocentric.Apis.Admin" /> assembly.
/// </summary>
public sealed class AdminApiEndpoints : IEndpointMapper
{
    /// <summary>
    ///     Maps the versioned Admin API endpoints.
    /// </summary>
    /// <inheritdoc />
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("admin/api");

        apiGroup.Map<V0Endpoints>();
    }
}
