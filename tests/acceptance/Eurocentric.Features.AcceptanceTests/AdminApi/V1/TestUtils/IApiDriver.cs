using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public interface IApiDriver
{
    IWebAppFixtureRestClient RestClient { get; }

    IWebAppFixtureBackDoor BackDoor { get; }

    IRestRequestFactory RequestFactory { get; }
}
