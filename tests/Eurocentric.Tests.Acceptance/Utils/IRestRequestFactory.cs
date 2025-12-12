using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

public interface IRestRequestFactory
{
    string ApiKey { get; }

    string ApiVersion { get; }

    RestRequest GetRequest(string route);

    RestRequest DeleteRequest(string route);

    RestRequest PatchRequest(string route);

    RestRequest PostRequest(string route);
}
