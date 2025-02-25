using Asp.Versioning;

namespace Eurocentric.Shared.ApiRegistration;

/// <summary>
///     Contains descriptive properties for an endpoint.
/// </summary>
public interface IEndpointInfo
{
    /// <summary>
    ///     Gets the request handling delegate for the endpoint.
    /// </summary>
    public Delegate Handler { get; }

    /// <summary>
    ///     Gets the relative path to the resource for the endpoint.
    /// </summary>
    public string Resource { get; }

    /// <summary>
    ///     Gets the HTTP method for the endpoint.
    /// </summary>
    public HttpMethod Method { get; }

    /// <summary>
    ///     Gets the unique ID for the endpoint.
    /// </summary>
    public string EndpointId { get; }

    /// <summary>
    ///     Gets the initial API version in which the endpoint is defined.
    /// </summary>
    /// <remarks>
    ///     The endpoint will be registered with all versions of the API with the same major version number and an equal
    ///     or later minor version number.
    /// </remarks>
    public ApiVersion InitialApiVersion { get; }

    /// <summary>
    ///     Gets the OpenAPI tag for the endpoint.
    /// </summary>
    public string Tag { get; }

    /// <summary>
    ///     Gets the OpenAPI summary for the endpoint.
    /// </summary>
    public string Summary { get; }

    /// <summary>
    ///     Gets the OpenAPI description for the endpoint.
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     Gets a list of all the ProblemDetails status codes returned by the endpoint.
    /// </summary>
    public IEnumerable<int> ProblemStatusCodes { get; }

    /// <summary>
    ///     Gets examples of the endpoint's request and/or response types.
    /// </summary>
    public IEnumerable<object> Examples { get; }
}
