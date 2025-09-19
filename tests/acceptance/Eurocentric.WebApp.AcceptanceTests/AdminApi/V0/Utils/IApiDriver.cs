using Eurocentric.WebApp.AcceptanceTests.Utils;

namespace Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils;

public interface IApiDriver
{
    IRestRequestFactory RequestFactory { get; }

    IWebAppFixtureRestClient RestClient { get; }

    IWebAppFixtureBackDoor BackDoor { get; }
}
