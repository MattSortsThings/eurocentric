using Asp.Versioning;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.V0;

internal static class Version0Point1Release
{
    private const string GroupName = "public-api-v0.1";

    internal static IEndpointRouteBuilder MapVersion0Point1Release(this IEndpointRouteBuilder api)
    {
        RouteGroupBuilder versionGroup = api.MapGroup("v{version:apiVersion}")
            .HasApiVersion(new ApiVersion(0, 1))
            .WithGroupName(GroupName);

        versionGroup.MapGetGreetings();

        return api;
    }

    internal static IServiceCollection AddVersion0Point1OpenApiDocument(this IServiceCollection services) =>
        services.AddOpenApi(GroupName);
}
