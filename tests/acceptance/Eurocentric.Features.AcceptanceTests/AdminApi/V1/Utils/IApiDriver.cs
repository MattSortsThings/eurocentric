using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public interface IApiDriver
{
    IWebAppFixtureRestClient RestClient { get; }

    IWebAppFixtureBackDoor BackDoor { get; }

    IRestRequestFactory RequestFactory { get; }
}
