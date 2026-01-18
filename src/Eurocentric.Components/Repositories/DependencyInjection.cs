using Eurocentric.Components.Repositories.Placeholders;
using Eurocentric.Domain.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Repositories;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds the repository services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The original <see cref="IServiceCollection" /> instance.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddPlaceholderRepositories();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
