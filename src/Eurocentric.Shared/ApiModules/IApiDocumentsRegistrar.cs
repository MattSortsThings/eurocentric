using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ApiModules;

/// <summary>
///     Defines a method for registering OpenAPI documents.
/// </summary>
public interface IApiDocumentsRegistrar
{
    /// <summary>
    ///     Adds all the OpenAPI documents for the API to the application service descriptor collection.
    /// </summary>
    /// <remarks>Register an implementation of this interface as a transient service in each API project.</remarks>
    /// <param name="services">Contains service descriptors for the application.</param>
    public void AddOpenApiDocuments(IServiceCollection services);
}
