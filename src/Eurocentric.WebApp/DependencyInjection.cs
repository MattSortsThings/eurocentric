using System.Reflection;
using Eurocentric.AdminApi;
using Eurocentric.PublicApi;
using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.Json;
using Eurocentric.Shared.Mapping;
using Eurocentric.Shared.OpenApi;
using Eurocentric.Shared.Security;
using Eurocentric.Shared.Versioning;

namespace Eurocentric.WebApp;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Configures all the services for the web application to be built.
    /// </summary>
    /// <param name="builder">The web application builder to be modified.</param>
    /// <returns>The same <see cref="WebApplicationBuilder" /> instance, so that method invocations can be chained.</returns>
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        Assembly[] apiAssemblies =
        [
            typeof(AdminApiModule).Assembly,
            typeof(PublicApiModule).Assembly
        ];

        builder.Services
            .AddTransient<IApiModule, AdminApiModule>()
            .AddTransient<IApiModule, PublicApiModule>()
            .AddAppPipeline(apiAssemblies)
            .AddMapping(apiAssemblies)
            .AddHttpJsonConfiguration()
            .AddOpenApiDocuments()
            .AddSecurity()
            .AddVersioning();

        return builder;
    }
}
