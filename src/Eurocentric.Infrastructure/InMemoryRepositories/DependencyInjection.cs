using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Infrastructure.InMemoryRepositories;

internal static class DependencyInjection
{
    internal static IServiceCollection AddInMemoryRepositories(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryContestRepository>()
            .AddSingleton<InMemoryQueryableRepository>();

        return services;
    }
}
