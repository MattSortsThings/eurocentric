using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

[Trait("Category", "ContainerTest")]
[Trait("Category", "AcceptanceTest")]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class AcceptanceTest(CleanWebAppFixture fixture) : IDisposable
{
    public void Dispose()
    {
        fixture.Reset();
        GC.SuppressFinalize(this);
    }

    private protected async Task<RestResponse<T>> SendAsync<T>(RestRequest request)
    {
        RestRequest restRequest = request;

        Func<IServiceProvider, Task<RestResponse<T>>> func = async provider =>
        {
            await using AsyncServiceScope scope = provider.CreateAsyncScope();
            IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

            return await client.ExecuteAsync<T>(restRequest, TestContext.Current.CancellationToken);
        };

        return await fixture.ExecuteScopedAsync(func);
    }

    private protected static RestRequest Get(string route) => new RestRequest(route)
        .AddHeader("Accept", "application/json, application/problem+json");

    private protected static RestRequest Post(string route) => new RestRequest(route, Method.Post)
        .AddHeader("Accept", "application/json, application/problem+json")
        .AddHeader("Content-Type", "application/json");
}
