using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.Documentation;

/// <summary>
///     Defines a method for generating OpenAPI documents.
/// </summary>
/// <remarks>Register an implementation of this interface as a transient service in each API project.</remarks>
public interface IOpenApiGenerator
{
    public void AddOpenApiDocuments(IServiceCollection services);
}
