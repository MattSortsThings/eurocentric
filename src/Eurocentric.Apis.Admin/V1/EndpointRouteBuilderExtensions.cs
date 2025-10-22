using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V1;

internal static class EndpointRouteBuilderExtensions
{
    internal static void MapV1EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v0Group = routeBuilder
            .MapGroup("v{version:apiVersion}")
            .WithGroupName(V1Group.Name)
            .RequiresAuthenticatedClient()
            .RequiresAdministratorRole();

        v0Group.Map<GetCountries.EndpointMapper>().Map<GetCountry.EndpointMapper>();
    }
}
