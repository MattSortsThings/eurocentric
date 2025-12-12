using Eurocentric.Apis.Admin.V0.Countries;
using Eurocentric.Components.Endpoints;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .Map<CreateCountryV0Point1.Endpoint>()
            .Map<DeleteCountryV0Point1.Endpoint>()
            .Map<GetCountriesV0Point1.Endpoint>()
            .Map<GetCountryV0Point1.Endpoint>();

        routeBuilder
            .Map<CreateCountryV0Point2.Endpoint>()
            .Map<DeleteCountryV0Point2.Endpoint>()
            .Map<GetCountriesV0Point2.Endpoint>()
            .Map<GetCountryV0Point2.Endpoint>();
    }
}
