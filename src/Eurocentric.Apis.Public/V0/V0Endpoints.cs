using Eurocentric.Apis.Public.V0.Common.Constants;
using Eurocentric.Apis.Public.V0.Placeholders;
using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder.MapGroup("v0").WithGroupName(EndpointGroup.Name);

        v0Group.Map<GetBlobbies.Endpoint>();
    }
}
