using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

public abstract class AdminActor<TResponse> : Actor<TResponse>, IAdminActor
    where TResponse : class
{
    protected AdminActor(IWebAppFixtureRestClient restClient, IWebAppFixtureBackDoor backDoor, string apiVersion = "v1.0") :
        base(restClient, backDoor)
    {
        RequestFactory = new AdminApiV1RequestFactory(apiVersion);
    }

    public IAdminApiV1RequestFactory RequestFactory { get; }

    public List<Country> GivenCountries { get; } = [];
}
