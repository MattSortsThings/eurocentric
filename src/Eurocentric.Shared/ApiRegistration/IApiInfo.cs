namespace Eurocentric.Shared.ApiRegistration;

/// <summary>
///     Contains descriptive properties for an API.
/// </summary>
public interface IApiInfo
{
    /// <summary>
    ///     Gets the unique name for the API.
    /// </summary>
    /// <remarks>
    ///     This name is only used internally in the web application. It is not used as part of any external description
    ///     of the API, e.g. OpenAPI documents.
    /// </remarks>
    /// <example>
    ///     <c>"StationsApi"</c>
    /// </example>
    public string ApiId { get; }

    /// <summary>
    ///     Gets the URL prefix for all the API endpoints.
    /// </summary>
    /// <remarks>
    ///     The prefix must not include any versioning information. A version number segment will be appended to the
    ///     prefix during endpoint registration.
    /// </remarks>
    /// <example>
    ///     <c>"stations/api"</c>
    /// </example>
    public string UrlPrefix { get; }

    /// <summary>
    ///     Gets the group name for all the API endpoints.
    /// </summary>
    /// <remarks>
    ///     The group name must be a URL-compatible string of lower-case letters and hyphens. Each OpenAPI document to be
    ///     generated for the API will have a unique name beginning with the endpoint prefix.
    /// </remarks>
    /// <example>
    ///     <c>"stations-api"</c>
    /// </example>
    public string EndpointGroupName { get; }

    /// <summary>
    ///     Gets the authorization policy name for the API.
    /// </summary>
    /// <example>
    ///     <c>"StationsApiAuthorizationPolicy"</c>
    /// </example>
    public string AuthorizationPolicyName { get; }

    /// <summary>
    ///     Gets the title for each OpenAPI document for the API.
    /// </summary>
    public string OpenApiDocumentTitle { get; }

    /// <summary>
    ///     Gets the description for each OpenAPI document for the API.
    /// </summary>
    public string OpenApiDocumentDescription { get; }
}
