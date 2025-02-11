using System.Text.Json;
using System.Text.Json.Serialization;
using Eurocentric.AdminApi;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.DataAccess;
using Eurocentric.PublicApi;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;

namespace Eurocentric.WebApp;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Configures all the services for the web application to be built.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The same <see cref="WebApplicationBuilder" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAppPipeline()
            .AddDataAccess()
            .ConfigureHttpJsonOptions();

        return builder;
    }

    /// <summary>
    ///     Configures the HTTP request pipeline for the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The same <see cref="WebApplication" /> instance, so that method invocations can be chained.</returns>
    internal static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.MapAdminApiPlaceholderEndpoint();
        app.MapPublicApiPlaceholderEndpoint();

        return app;
    }

    private static IServiceCollection AddAppPipeline(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<IAdminApiMarker>()
            .RegisterServicesFromAssemblyContaining<IPublicApiMarker>());

        return services;
    }

    private static IServiceCollection ConfigureHttpJsonOptions(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        return services;
    }
}
