using Eurocentric.Apis.Admin.V0.Features.Placeholders;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder.MapGroup("v0");

        v0Group.Map<PingAdminApi.Endpoint>();
    }
}
