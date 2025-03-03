using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.Tests.Integration.Utils;

[Trait("Category", "ContainerTest")]
[Trait("Category", "IntegrationTest")]
public abstract class IntegrationTest(SeededWebAppFixture fixture)
{
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
