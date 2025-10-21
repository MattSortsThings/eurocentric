using Eurocentric.Apis.Public.V0.OpenApi;
using Eurocentric.Components.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Apis.Public;

public static class OpenApiDocuments
{
    public static void RegisterAll(IServiceCollection services)
    {
        services
            .RegisterOpenApiDocument<V0Point1DocumentRegistrar>()
            .RegisterOpenApiDocument<V0Point2DocumentRegistrar>();
    }
}
