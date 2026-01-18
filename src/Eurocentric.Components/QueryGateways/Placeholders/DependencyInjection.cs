using Eurocentric.Domain.Queries.Placeholders;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.QueryGateways.Placeholders;

internal static class DependencyInjection
{
    internal static void AddPlaceholderQueryGateways(this IServiceCollection services) =>
        services.AddScoped<IQueryablesGateway, PlaceholderQueryablesGateway>();
}
