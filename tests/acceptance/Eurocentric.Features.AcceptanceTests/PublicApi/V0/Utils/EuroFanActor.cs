using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public abstract class EuroFanActor<TResponse> : Actor<TResponse>, IEuroFanActor where TResponse : class
{
    protected EuroFanActor(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
        base(restClient, backDoor)
    {
        RequestFactory = new PublicApiV0RequestFactory(apiVersion);
    }

    public IPublicApiV0RequestFactory RequestFactory { get; }
}
