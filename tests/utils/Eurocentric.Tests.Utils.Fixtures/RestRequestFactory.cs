using RestSharp;

namespace Eurocentric.Tests.Utils.Fixtures;

/// <summary>
///     Factory methods for the <see cref="RestRequest" /> class.
/// </summary>
public static class RestRequestFactory
{
    /// <summary>
    ///     Creates and returns a new <see cref="RestRequest" /> object representing a GET request to the specified resource
    ///     URI.
    /// </summary>
    /// <remarks>
    ///     The request is pre-configured as follows:
    ///     <list type="bullet">
    ///         <item>Its HTTP method is GET, and</item>
    ///         <item>it has an "Accept" request header with the value "application/json".</item>
    ///     </list>
    /// </remarks>
    /// <param name="resourceUri">
    ///     The resource URI to use for the request. This will be combined with the base address of the HTTP client when the
    ///     request is handled.
    /// </param>
    /// <returns>A new <see cref="RestRequest" /> instance.</returns>
    public static RestRequest Get(string resourceUri) =>
        new RestRequest(resourceUri)
            .AddHeader("Accept", "application/json");

    /// <summary>
    ///     Creates and returns a new <see cref="RestRequest" /> object representing a POST request to the specified resource
    ///     URI.
    /// </summary>
    /// <remarks>
    ///     The request is pre-configured as follows:
    ///     <list type="bullet">
    ///         <item>Its HTTP method is POST, and</item>
    ///         <item>it has an "Accept" request header with the value "application/json", and</item>
    ///         <item>it has a "Content-Type" request header with the value "application/json".</item>
    ///     </list>
    /// </remarks>
    /// <param name="resourceUri">
    ///     The resource URI to use for the request. This will be combined with the base address of the HTTP client when the
    ///     request is handled.
    /// </param>
    /// <returns>A new <see cref="RestRequest" /> instance.</returns>
    public static RestRequest Post(string resourceUri) =>
        new RestRequest(resourceUri, Method.Post)
            .AddHeader("Accept", "application/json")
            .AddHeader("Content-Type", "application/json");
}
