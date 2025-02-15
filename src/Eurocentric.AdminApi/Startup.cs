using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
        services.AddTransient<Action<MediatRServiceConfiguration>>(_ => configuration =>
            configuration.RegisterServicesFromAssemblyContaining(typeof(Startup)));

        services.AddTransient<Action<IEndpointRouteBuilder>>(_ => builder => builder.MapGet("admin/api/v0.1/message",
            () => TypedResults.Ok($"Admin API zapped to the extreme at {DateTime.Now}!")));

        return services;
    }
}
