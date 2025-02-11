using Asp.Versioning;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi.V0;

internal static class Version0Point1Release
{
    private const string GroupName = "admin-api-v0.1";

    internal static IEndpointRouteBuilder MapVersion0Point1Release(this IEndpointRouteBuilder api)
    {
        RouteGroupBuilder versionGroup = api.MapGroup("v{version:apiVersion}")
            .HasApiVersion(new ApiVersion(0, 1))
            .WithGroupName(GroupName);

        versionGroup.MapCreateCalculationEndpoint();

        return api;
    }

    internal static IServiceCollection AddVersion0Point1OpenApiDocument(this IServiceCollection services) =>
        services.AddOpenApi(GroupName);
}
