using Eurocentric.Apis.Public.V0.Common.Configuration;
using Eurocentric.Apis.Public.V0.Contests;
using Eurocentric.Apis.Public.V0.Countries;
using Eurocentric.Components.EndpointMapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public.V0;

internal sealed class V0Endpoints : IEndpointMapper
{
    public void Map(IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder
            .MapGroup("/")
            .WithGroupName(EndpointGroup.Name)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v0Group.Map<GetQueryableContestsV0Point1.Endpoint>().Map<GetQueryableCountriesV0Point1.Endpoint>();
    }
}
