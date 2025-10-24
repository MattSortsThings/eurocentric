using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Apis.Admin.V0.Features.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0;

internal static class EndpointRouteBuilderExtensions
{
    internal static void MapV0EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v0Group = routeBuilder
            .MapGroup("v{version:apiVersion}")
            .WithGroupName(V0Group.Name)
            .RequiresAuthenticatedClient()
            .RequiresAdministratorRole()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        v0Group
            .Map<CreateCountry.EndpointMapper>()
            .Map<DeleteCountry.EndpointMapper>()
            .Map<GetCountries.EndpointMapper>()
            .Map<GetCountry.EndpointMapper>();
    }
}
