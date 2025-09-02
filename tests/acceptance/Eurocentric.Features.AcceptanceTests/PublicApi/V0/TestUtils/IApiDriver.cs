using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

public interface IApiDriver
{
    IWebAppFixtureRestClient RestClient { get; }

    IWebAppFixtureBackDoor BackDoor { get; }

    IRestRequestFactory RequestFactory { get; }
}
