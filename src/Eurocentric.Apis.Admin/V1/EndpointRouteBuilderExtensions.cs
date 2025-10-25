using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V1;

internal static class EndpointRouteBuilderExtensions
{
    internal static void MapV1EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v1Group = routeBuilder
            .MapGroup("v{version:apiVersion}")
            .WithGroupName(V1Group.Name)
            .RequiresAuthenticatedClient()
            .RequiresAdministratorRole()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        v1Group.Map<CreateCountry.EndpointMapper>().Map<GetCountries.EndpointMapper>().Map<GetCountry.EndpointMapper>();
    }
}
