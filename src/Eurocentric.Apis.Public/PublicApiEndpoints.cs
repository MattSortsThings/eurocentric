using Eurocentric.Apis.Public.V0;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Maps the endpoints declared in the <see cref="Eurocentric.Apis.Public" /> assembly.
/// </summary>
[Mapper]
public sealed partial class PublicApiEndpoints : IEndpointMapper
{
    /// <inheritdoc />
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("public/api");

        apiGroup.Map<V0Endpoints>();
    }
}
