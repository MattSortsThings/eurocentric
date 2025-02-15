using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Microsoft.AspNetCore.Routing;
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
        services.AddTransient<Action<MediatRServiceConfiguration>>(_ => configuration =>
            configuration.RegisterServicesFromAssemblyContaining(typeof(Startup)));

        services.AddTransient<Action<IEndpointRouteBuilder>>(_ => builder => builder.MapGetGreetings());

        return services;
    }
}
