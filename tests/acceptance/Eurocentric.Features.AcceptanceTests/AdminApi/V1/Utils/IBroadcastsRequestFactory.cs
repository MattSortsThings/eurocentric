using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

/// <summary>
///     Creates REST requests for "Broadcasts" tagged endpoints.
/// </summary>
public interface IBroadcastsRequestFactory
{
    /// <summary>
    ///     Creates a REST request for the "DeleteBroadcast" endpoint.
    /// </summary>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest DeleteBroadcast(Guid broadcastId);

    /// <summary>
    ///     Creates a REST request for the "GetBroadcast" endpoint.
    /// </summary>
    /// <param name="broadcastId">The broadcast ID.</param>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest GetBroadcast(Guid broadcastId);

    /// <summary>
    ///     Creates a REST request for the "GetBroadcasts" endpoint.
    /// </summary>
    /// <returns>A <see cref="RestRequest" /> instance.</returns>
    public RestRequest GetBroadcasts();
}
