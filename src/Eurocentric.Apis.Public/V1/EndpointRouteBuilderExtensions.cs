using Eurocentric.Apis.Public.V1.Config;
using Eurocentric.Apis.Public.V1.Features.Queryables;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V1;

internal static class EndpointRouteBuilderExtensions
{
    internal static void MapV1EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v1Group = routeBuilder
            .MapGroup("v{version:apiVersion}")
            .WithGroupName(V1Group.Name)
            .RequiresAuthenticatedClient()
            .RequiresUserRole()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v1Group
            .Map<GetQueryableBroadcasts.EndpointMapper>()
            .Map<GetQueryableContests.EndpointMapper>()
            .Map<GetQueryableCountries.EndpointMapper>();
    }
}
