using Eurocentric.Tests.Acceptance.Utils.Contracts;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public abstract partial class EuroFan
{
    private sealed class EuroFanKernel(ITestWebApp testWebApp, IPublicApiV0RestRequestFactory requestFactory)
        : IEuroFanKernel
    {
        public ITestWebAppBackDoor BackDoor { get; } = testWebApp;

        public ITestWebAppRestClient RestClient { get; } = testWebApp;

        public IPublicApiV0RestRequestFactory RequestFactory { get; } = requestFactory;
    }
}
