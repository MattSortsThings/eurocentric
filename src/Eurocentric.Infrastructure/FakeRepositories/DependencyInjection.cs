using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.FakeRepositories;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    /// <summary>
    ///     Adds the fake data repository services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddFakeDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<FakeVoterPointsDataRepository>();

        return services;
    }
}
