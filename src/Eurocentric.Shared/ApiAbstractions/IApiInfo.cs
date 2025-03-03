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
    ///     The URL prefix must include an API version segment template as shown in the example.
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
}
