using Eurocentric.Apis.Public.V0.Features.Placeholders;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder.MapGroup("v0");

        v0Group.Map<PingPublicApi.Endpoint>();
    }
}
