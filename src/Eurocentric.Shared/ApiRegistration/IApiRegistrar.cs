using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ApiRegistration;

/// <summary>
///     Defines a contract for registering a versioned API with OpenAPI documents.
/// </summary>
public interface IApiRegistrar
{
    /// <summary>
    ///     Maps all the versioned endpoints for the API as endpoints on the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    public void MapVersionedEndpoints(IEndpointRouteBuilder app);

    /// <summary>
    ///     Adds all the OpenAPI documents for the API to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    public void AddOpenApiDocuments(IServiceCollection services);
}
