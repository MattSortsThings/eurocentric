using RestSharp;

namespace Eurocentric.Tests.Acceptance.PublicApi.V0.Utils;

public interface IPublicApiV0RestRequestFactory
{
    string ApiVersion { get; }

    string ApiKey { get; }

    RestRequest GetQueryableBroadcasts();

    RestRequest GetQueryableContests();

    RestRequest GetQueryableCountries();
}
