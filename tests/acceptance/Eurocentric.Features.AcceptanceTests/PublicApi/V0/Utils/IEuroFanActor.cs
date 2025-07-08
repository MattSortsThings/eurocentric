using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public interface IEuroFanActor
{
    public IWebAppFixtureBackDoor BackDoor { get; }

    public IWebAppFixtureRestClient RestClient { get; }

    public IPublicApiV0RequestFactory RequestFactory { get; }
}
