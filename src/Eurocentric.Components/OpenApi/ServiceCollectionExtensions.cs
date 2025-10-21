using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.OpenApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterOpenApiDocument<T>(this IServiceCollection services)
        where T : IOpenApiDocumentRegistrar, new()
    {
        T registrar = new();

        registrar.Register(services);

        return services;
    }
}
