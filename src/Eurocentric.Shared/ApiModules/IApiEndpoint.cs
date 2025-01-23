using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiModules;

public interface IApiEndpoint
{
    /// <summary>
    ///     The major version of the API to which the endpoint belongs.
    /// </summary>
    public int MajorVersion { get; }

    /// <summary>
    ///     The minor version of the API at which the endpoint was introduced.
    /// </summary>
    public int MinorVersion { get; }

    /// <summary>
    ///     The display name for the endpoint.
    /// </summary>
    /// <remarks>The endpoint's display name is concatenated with its release group name to generate a globally unique name.</remarks>
    public string DisplayName { get; }

    /// <summary>
    ///     Maps the endpoint to the API release group.
    /// </summary>
    /// <param name="releaseGroup">The API release group.</param>
    /// <returns>A <see cref="RouteHandlerBuilder" /> object that can be used to configure the mapped endpoint further.</returns>
    public RouteHandlerBuilder Map(IEndpointRouteBuilder releaseGroup);
}
