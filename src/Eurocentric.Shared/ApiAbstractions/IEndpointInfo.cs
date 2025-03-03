namespace Eurocentric.Shared.ApiAbstractions;

/// <summary>
///     Contains descriptors for an endpoint.
/// </summary>
public interface IEndpointInfo
{
    /// <summary>
    ///     Gets the unique name for the endpoint.
    /// </summary>
    /// <example>
    ///     <c>"GetSong"</c>
    /// </example>
    public string Name { get; }

    /// <summary>
    ///     Gets the request handler delegate for the endpoint.
    /// </summary>
    public Delegate Handler { get; }

    /// <summary>
    ///     Gets the HTTP method for the endpoint.
    /// </summary>
    public HttpMethod Method { get; }

    /// <summary>
    ///     Gets the relative route pattern for the endpoint.
    /// </summary>
    /// <example>"songs/{id}"</example>
    public string Route { get; }

    /// <summary>
    ///     Gets the major API version number in which the endpoint was added.
    /// </summary>
    public int MajorApiVersion { get; }

    /// <summary>
    ///     Gets the minor API version number in which the endpoint was added.
    /// </summary>
    public int MinorApiVersion { get; }
}
