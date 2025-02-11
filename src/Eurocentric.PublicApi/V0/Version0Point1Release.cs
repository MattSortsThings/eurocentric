using Asp.Versioning;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi.V0;

internal static class Version0Point1Release
{
    internal static IEndpointRouteBuilder MapVersion0Point1Release(this IEndpointRouteBuilder api)
    {
        RouteGroupBuilder versionGroup = api.MapGroup("v{version:apiVersion}")
            .HasApiVersion(new ApiVersion(0, 1))
            .WithGroupName("public-api-v0.1");

        versionGroup.MapGetGreetings();

        return api;
    }
}
