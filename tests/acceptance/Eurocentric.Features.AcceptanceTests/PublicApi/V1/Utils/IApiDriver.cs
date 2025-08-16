using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public interface IApiDriver
{
    IWebAppFixtureRestClient RestClient { get; }

    IRestRequestFactory RequestFactory { get; }
}
