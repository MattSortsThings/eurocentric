using Eurocentric.Tests.Acceptance.Utils.Contracts;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public interface IEuroFanKernel
{
    ITestWebAppBackDoor BackDoor { get; }

    ITestWebAppRestClient RestClient { get; }

    IPublicApiV0RestRequestFactory RequestFactory { get; }
}
