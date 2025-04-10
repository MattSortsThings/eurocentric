using SlimMessageBus;

namespace Eurocentric.TestUtils.WebAppFixtures;

public interface ITestMessageBus
{
    public Task<T> SendAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default);
}
