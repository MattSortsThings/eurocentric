using ErrorOr;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.PublicApi.Tests.Integration.Utils;

[IntegrationTest]
[Collection(nameof(SeededWebAppTestCollection))]
public abstract class IntegrationTest
{
    private readonly SeededWebAppFixture _fixture;

    protected IntegrationTest(SeededWebAppFixture fixture)
    {
        _fixture = fixture;
    }

    private protected async Task<ErrorOr<TResult>> SendAsync<TResult>(Query<TResult> query) =>
        await _fixture.SendAsync(query, TestContext.Current.CancellationToken);
}
