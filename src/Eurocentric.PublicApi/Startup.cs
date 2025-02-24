using Eurocentric.PublicApi.Common;
using Eurocentric.Shared.ApiRegistration;
using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the Public API services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddPublicApiServices(this IServiceCollection services)
    {
        services.AddTransient<IApiAuthorizationPolicy, PublicApiAuthorizationPolicy>()
            .AddTransient<IAppPipelineConfigurator, AppPipelineConfigurator>()
            .AddTransient<IApiRegistrar, ApiRegistrar<PublicApiInfo>>();

        return services;
    }
}
