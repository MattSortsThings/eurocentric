using Eurocentric.Apis.Admin.V0;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Maps the endpoints declared in the <see cref="Eurocentric.Apis.Admin" /> assembly.
/// </summary>
[Mapper]
public sealed partial class AdminApiEndpoints : IEndpointMapper
{
    /// <inheritdoc />
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder apiGroup = builder.MapGroup("admin/api");

        apiGroup.Map<V0Endpoints>();
    }
}
