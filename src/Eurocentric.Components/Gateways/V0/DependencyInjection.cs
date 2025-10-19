using Eurocentric.Domain.V0.Queries.Queryables;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.Gateways.V0;

internal static class DependencyInjection
{
    internal static IServiceCollection AddV0Gateways(this IServiceCollection services)
    {
        services.AddScoped<IQueryablesGateway, QueryablesGateway>();

        return services;
    }
}
