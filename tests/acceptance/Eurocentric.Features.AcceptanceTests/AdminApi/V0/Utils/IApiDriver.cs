using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public interface IApiDriver
{
    public IWebAppFixtureRestClient RestClient { get; }

    public IWebAppFixtureBackDoor BackDoor { get; }

    public IRestRequestFactory RequestFactory { get; }
}
