namespace Eurocentric.Shared.ApiAbstractions;

/// <summary>
///     Contains descriptors for an API.
/// </summary>
public interface IApiInfo
{
    /// <summary>
    ///     Gets the URL prefix for the API.
    /// </summary>
    /// <example>
    ///     <c>"music/api"</c>
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
