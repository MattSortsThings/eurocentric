using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public interface IWebAppFixtureRestClient
{
    Task<BiRestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default);

    Task<BiRestResponse<TResponse>> SendAsync<TResponse>(RestRequest request, CancellationToken cancellationToken = default)
        where TResponse : class;
}
