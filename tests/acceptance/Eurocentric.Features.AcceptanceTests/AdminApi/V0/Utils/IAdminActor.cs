using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public interface IAdminActor
{
    public IWebAppFixtureBackDoor BackDoor { get; }

    public IWebAppFixtureRestClient RestClient { get; }

    public IAdminApiV0RequestFactory RequestFactory { get; }

    public List<Contest> GivenContests { get; }
}
