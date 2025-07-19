using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Contests;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IAdminActor : IResponseVerifier
{
    public static readonly ContestParticipantSpecification DefaultContestParticipantSpecification =
        new(Guid.Empty, "ActName", "SongTitle");

    /// <summary>
    ///     Broadcasts created in the system during "given" scenario steps.
    /// </summary>
    public BroadcastCollection GivenBroadcasts { get; }

    /// <summary>
    ///     Contests created in the system during "given" scenario steps.
    /// </summary>
    public ContestCollection GivenContests { get; }

    /// <summary>
    ///     Countries created in the system during "given" scenario steps.
    /// </summary>
    public CountryCollection GivenCountries { get; }

    /// <summary>
    ///     Allows the client to modify the state of the in-memory web app fixture at runtime using scoped operations on the
    ///     fixture's service provider.
    /// </summary>
    public IWebAppFixtureBackDoor BackDoor { get; }

    /// <summary>
    ///     Sends a REST request to the in-memory web application fixture and returns its response.
    /// </summary>
    public IWebAppFixtureRestClient RestClient { get; }

    /// <summary>
    ///     Creates REST requests for version 1 of the AdminActor API.
    /// </summary>
    public IRequestFactory RequestFactory { get; }
}
