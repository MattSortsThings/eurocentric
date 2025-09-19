using Eurocentric.WebApp.AcceptanceTests.Utils;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public interface IApiDriver
{
    IWebAppFixtureRestClient RestClient { get; }

    IRestRequestFactory RequestFactory { get; }
}
