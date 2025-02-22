using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.AppPipeline;
using Eurocentric.Shared.Documentation;
using Eurocentric.Shared.ErrorHandling;
using Eurocentric.Shared.Json;
using Eurocentric.Shared.Security;
using Eurocentric.Shared.Timing;
using Eurocentric.Shared.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Eurocentric.Shared;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     Adds the shared services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddAppPipeline()
            .AddDocumentation()
            .AddErrorHandling()
            .AddJsonOptionsConfiguration()
            .AddSecurity()
            .AddTimeProvider()
            .AddVersioning();

        return services;
    }

    /// <summary>
    ///     Configures the web app to use API documentation endpoints.
    /// </summary>
    /// <remarks>All documentation endpoints allow anonymous requests.</remarks>
    /// <param name="app">The web application.</param>
    public static void UseDocumentationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi()
            .AllowAnonymous();

        app.MapScalarApiReference("docs")
            .AllowAnonymous();
    }

    /// <summary>
    ///     Configures the web app to use the API endpoints that have been registered.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiEndpoints(this IEndpointRouteBuilder app)
    {
        using IServiceScope scope = app.ServiceProvider.CreateScope();

        foreach (IApiEndpointsMapper mapper in scope.ServiceProvider.GetRequiredService<IEnumerable<IApiEndpointsMapper>>())
        {
            mapper.Map(app);
        }
    }
}
