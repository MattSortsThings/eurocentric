namespace Eurocentric.Tests.Acceptance.Utils;

public interface IActorKernel
{
    IWebAppBackDoor BackDoor { get; }

    IWebAppRestClient RestClient { get; }

    IRestRequestFactory RestRequestFactory { get; }
}
