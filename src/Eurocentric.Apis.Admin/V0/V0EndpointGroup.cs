using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Apis.Admin.V0.Features.Countries;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0;

internal static class V0EndpointGroup
{
    internal static void MapV0EndpointGroup(this IEndpointRouteBuilder routeBuilder)
    {
        RouteGroupBuilder v0Group = routeBuilder
            .MapGroup("/")
            .WithGroupName(EndpointConstants.GroupName)
            .AllowAnonymous();

        v0Group
            .Map<CreateCountryV0Point1.EndpointMapper>()
            .Map<DeleteCountryV0Point1.EndpointMapper>()
            .Map<GetCountriesV0Point1.EndpointMapper>()
            .Map<GetCountryV0Point1.EndpointMapper>();
    }
}
