using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiMapping;

/// <summary>
///     Defines a method for mapping all the endpoints for an API.
/// </summary>
/// <remarks>Register an implementation of this interface as a transient service in each API project.</remarks>
public interface IApiEndpointsMapper
{
    /// <summary>
    ///     Maps all the endpoints for the API.
    /// </summary>
    /// <param name="app">The web application.</param>
    public void Map(IEndpointRouteBuilder app);
}
