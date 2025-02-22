using Eurocentric.AdminApi.Common;
using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the Admin API services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddAdminApiServices(this IServiceCollection services)
    {
        services.AddTransient<IApiAuthorizationPolicy, AdminApiAuthorizationPolicy>()
            .AddTransient<IApiDocumentsRegistrar, AdminApiModule>()
            .AddTransient<IApiEndpointsMapper, AdminApiModule>()
            .AddTransient<IAppPipelineConfigurator, AppPipelineConfigurator>();

        return services;
    }
}
