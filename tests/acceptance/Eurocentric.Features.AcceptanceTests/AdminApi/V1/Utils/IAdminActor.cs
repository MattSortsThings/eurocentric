using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IAdminActor
{
    public IWebAppFixtureBackDoor BackDoor { get; }

    public IWebAppFixtureRestClient RestClient { get; }

    public IAdminApiV1RequestFactory RequestFactory { get; }

    public List<Broadcast> GivenBroadcasts { get; }

    public List<Contest> GivenContests { get; }

    public CountryLookup GivenCountries { get; }
}
