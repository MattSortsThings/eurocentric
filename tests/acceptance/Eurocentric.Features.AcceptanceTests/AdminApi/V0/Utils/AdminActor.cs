using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public abstract class AdminActor<TResponse> : Actor<TResponse>, IAdminActor where TResponse : class
{
    protected AdminActor(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
        base(restClient, backDoor)
    {
        RequestFactory = new AdminApiV0RequestFactory(apiVersion);
    }

    public IAdminApiV0RequestFactory RequestFactory { get; }

    public List<Contest> GivenContests { get; } = [];
}
