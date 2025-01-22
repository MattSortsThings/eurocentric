using RestSharp;

namespace Eurocentric.Tests.Utils.Fixtures;

public interface ITestableWebApi
{
    public Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default);

    public Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default);
}
