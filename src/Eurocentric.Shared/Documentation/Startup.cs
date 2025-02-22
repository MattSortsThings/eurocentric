using Eurocentric.Shared.ApiModules;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Documentation;

internal static class Startup
{
    internal static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        foreach (IApiDocumentsRegistrar registrar in serviceProvider.GetRequiredService<IEnumerable<IApiDocumentsRegistrar>>())
        {
            registrar.AddOpenApiDocuments(services);
        }

        services.ConfigureOptions<ScalarOptionsConfigurator>();

        return services;
    }
}
