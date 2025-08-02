using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public interface IApiDriver
{
    public IWebAppFixtureRestClient RestClient { get; }

    public IRestRequestFactory RequestFactory { get; }
}
