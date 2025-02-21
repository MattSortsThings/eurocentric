using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Timing;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds the system time provider to the application service descriptor collection as an injectable singleton service
    ///     of type <see cref="TimeProvider" />.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddTimeProvider(this IServiceCollection services) =>
        services.AddSingleton<TimeProvider>(TimeProvider.System);
}
