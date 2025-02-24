using Eurocentric.Shared.ApiRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Documentation;

internal static class Startup
{
    internal static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.ConfigureOptions<ScalarOptionsConfigurator>();
        foreach (IApiRegistrar registrar in GetApiRegistrars(services))
        {
            registrar.AddOpenApiDocuments(services);
        }

        return services;
    }

    private static IApiRegistrar[] GetApiRegistrars(IServiceCollection services)
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        using IServiceScope scope = serviceProvider.CreateScope();

        return scope.ServiceProvider.GetRequiredService<IEnumerable<IApiRegistrar>>().ToArray();
    }
}
