using ErrorOr;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.Tests.Integration.Utils;

[IntegrationTest]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class IntegrationTest : IDisposable
{
    private readonly CleanWebAppFixture _fixture;

    protected IntegrationTest(CleanWebAppFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.Reset();
        GC.SuppressFinalize(this);
    }

    private protected async Task<ErrorOr<TResult>> SendAsync<TResult>(Command<TResult> command) =>
        await _fixture.SendAsync(command, TestContext.Current.CancellationToken);
}
