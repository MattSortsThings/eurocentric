namespace Eurocentric.Shared.ApiAbstractions;

/// <summary>
///     Contains descriptors for an API.
/// </summary>
public interface IApiInfo
{
    /// <summary>
    ///     Gets the unique identifier for the API.
    /// </summary>
    /// <example>"MusicApi"</example>
    public string Id { get; }

    /// <summary>
    ///     Gets the URL prefix for the API.
    /// </summary>
    /// <remarks>
    ///     The URL prefix must include an API version segment template as shown in the example. This will be replaced with the
    ///     version number when endpoints are mapped.
    /// </remarks>
    /// <example>
    ///     <c>"music/api/v{version:apiVersion}"</c>
    /// </example>
    public string UrlPrefix { get; }

    /// <summary>
    ///     Gets the endpoint group name for the API.
    /// </summary>
    /// <example>
    ///     <c>"music-api"</c>
    /// </example>
    public string EndpointGroupName { get; }

    /// <summary>
    ///     Gets the sequence of problem status codes returned by every endpoint in the API.
    /// </summary>
    public IEnumerable<int> ProblemStatusCodes { get; }

    /// <summary>
    ///     Gets the OpenAPI document title for the API.
    /// </summary>
    public string Title { get; }

    /// <summary>
    ///     Gets the OpenAPI document description for the API.
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     Gets the authorization policy name for the API.
    /// </summary>
    public string AuthorizationPolicyName { get; }
}
