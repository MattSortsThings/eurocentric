using Eurocentric.Apis.Admin.V0.Common.Configuration;
using Eurocentric.Apis.Admin.V0.Countries;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder
            .MapGroup("/")
            .WithGroupName(EndpointGroup.Name)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        v0Group
            .Map<CreateCountryV0Point1.Endpoint>()
            .Map<DeleteCountryV0Point1.Endpoint>()
            .Map<GetCountryV0Point1.Endpoint>()
            .Map<GetCountriesV0Point1.Endpoint>();
    }
}
