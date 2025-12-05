using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

public interface ITestWebAppClient
{
    Task<RestResponse> SendAsync(RestRequest request);
}
