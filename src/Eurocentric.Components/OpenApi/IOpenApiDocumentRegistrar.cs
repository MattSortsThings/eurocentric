using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Components.OpenApi;

public interface IOpenApiDocumentRegistrar
{
    string DocumentName { get; }

    void Configure(OpenApiOptions options);

    void Register(IServiceCollection services) => services.AddOpenApi(DocumentName, Configure);
}
