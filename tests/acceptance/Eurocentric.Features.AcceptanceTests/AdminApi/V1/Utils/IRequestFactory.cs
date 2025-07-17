namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

/// <summary>
///     Creates REST requests for version 1 of the Admin API.
/// </summary>
public interface IRequestFactory
{
    /// <summary>
    ///     Creates REST requests for "Broadcasts" tagged endpoints.
    /// </summary>
    public IBroadcastsRequestFactory Broadcasts { get; }

    /// <summary>
    ///     Creates REST requests for "Contests" tagged endpoints.
    /// </summary>
    public IContestsRequestFactory Contests { get; }

    /// <summary>
    ///     Creates REST requests for "Countries" tagged endpoints.
    /// </summary>
    public ICountriesRequestFactory Countries { get; }
}
