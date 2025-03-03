using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi.Tests.Integration.Utils;

[Trait("Category", "ContainerTest")]
[Trait("Category", "IntegrationTest")]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class IntegrationTest(CleanWebAppFixture fixture) : IDisposable
{
    public void Dispose()
    {
        fixture.Reset();
        GC.SuppressFinalize(this);
    }

    private protected async Task<T> SendAsync<T>(IRequest<T> request)
    {
        IRequest<T> appRequest = request;

        Func<IServiceProvider, Task<T>> func = async provider =>
        {
            await using AsyncServiceScope scope = provider.CreateAsyncScope();
            ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

            return await sender.Send(appRequest, TestContext.Current.CancellationToken);
        };

        return await fixture.ExecuteScopedAsync(func);
    }
}
