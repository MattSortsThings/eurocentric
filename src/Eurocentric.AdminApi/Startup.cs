using Eurocentric.AdminApi.Common;
using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.Documentation;
using Eurocentric.Shared.Endpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi;

/// <summary>
///     Extension methods to be invoked on application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the Admin API services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAdminApi(this IServiceCollection services)
    {
        services.AddTransient<IAppPipelineConfigurator, AppPipelineConfigurator>()
            .AddTransient<IEndpointMapper, EndpointMapper<ApiInfo>>()
            .AddTransient<IOpenApiGenerator, OpenApiGenerator<ApiInfo>>();

        return services;
    }
}
