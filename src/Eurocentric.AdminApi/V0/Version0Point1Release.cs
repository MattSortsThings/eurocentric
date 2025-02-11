using Asp.Versioning;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0;

internal static class Version0Point1Release
{
    internal static IEndpointRouteBuilder MapVersion0Point1Release(this IEndpointRouteBuilder api)
    {
        RouteGroupBuilder versionGroup = api.MapGroup("v{version:apiVersion}")
            .HasApiVersion(new ApiVersion(0, 1))
            .WithGroupName("admin-api-v0.1");

        versionGroup.MapCreateCalculationEndpoint();

        return api;
    }
}
