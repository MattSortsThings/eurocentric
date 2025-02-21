using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Json;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds the JSON options configuration to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddJsonOptionsConfiguration(this IServiceCollection services) =>
        services.ConfigureOptions<JsonOptionsConfiguration>();
}
