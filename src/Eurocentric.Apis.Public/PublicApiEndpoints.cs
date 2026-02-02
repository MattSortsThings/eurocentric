using Eurocentric.Apis.Public.V0;
using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Maps all endpoints defined in the <see cref="Eurocentric.Apis.Public" /> assembly.
/// </summary>
public sealed class PublicApiEndpoints : IEndpointMapper
{
    /// <summary>
    ///     Maps the versioned Public API endpoints.
    /// </summary>
    /// <inheritdoc />
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("public/api");

        apiGroup.Map<V0Endpoints>();
    }
}
