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
    /// <example>
    ///     <c>"songs/{id}"</c>
    /// </example>
    public string Route { get; }

    /// <summary>
    ///     Gets the major API version number in which the endpoint was added.
    /// </summary>
    public int MajorApiVersion { get; }

    /// <summary>
    ///     Gets the minor API version number in which the endpoint was added.
    /// </summary>
    public int MinorApiVersion { get; }

    /// <summary>
    ///     Gets the OpenAPI summary for the endpoint.
    /// </summary>
    /// <example>
    ///     <c>"Get a song"</c>
    /// </example>
    public string Summary { get; }

    /// <summary>
    ///     Gets the OpenAPI description for the endpoint.
    /// </summary>
    /// <example>
    ///     <c>"Retrieves a single song. The client must supply the song ID as a route parameter."</c>
    /// </example>
    public string Description { get; }

    /// <summary>
    ///     Gets the OpenAPI tag for the endpoint.
    /// </summary>
    /// <example>
    ///     <c>"Songs"</c>
    /// </example>
    public string Tag { get; }

    /// <summary>
    ///     Gets the sequence of problem status codes returned by the endpoint.
    /// </summary>
    /// <remarks>This sequence should exclude all problem status codes defined at the API level.</remarks>
    public IEnumerable<int> ProblemStatusCodes { get; }

    /// <summary>
    ///     Gets the sequence of example request body and/or response body objects for the endpoint.
    /// </summary>
    /// <remarks>There can only be a single example of any given type.</remarks>
    public IEnumerable<object> Examples { get; }
}
