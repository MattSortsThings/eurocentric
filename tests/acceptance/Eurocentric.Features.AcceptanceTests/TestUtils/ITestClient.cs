using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public interface ITestClient
{
    public Task<ResponseOrProblem> SendRequestAsync(RestRequest request,
        CancellationToken cancellationToken = default);

    public Task<ResponseOrProblem<TResponse>> SendRequestAsync<TResponse>(RestRequest request,
        CancellationToken cancellationToken = default)
        where TResponse : class;
}
