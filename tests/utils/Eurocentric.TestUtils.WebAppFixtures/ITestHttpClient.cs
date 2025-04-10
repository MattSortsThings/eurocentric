using RestSharp;

namespace Eurocentric.TestUtils.WebAppFixtures;

public interface ITestHttpClient
{
    public Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default);

    public Task<RestResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default);
}
