using Eurocentric.TestUtils.Categories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.Tests.Integration.TestUtils;

[IntegrationTest]
[Collection(nameof(SeededWebAppFixtureTestCollection))]
public abstract class IntegrationTest : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly SeededWebAppFixture _webAppFixture;

    protected IntegrationTest(SeededWebAppFixture webAppFixture)
    {
        _webAppFixture = webAppFixture;
        _scope = webAppFixture.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    private protected ISender Sender { get; }

    public void Dispose()
    {
        _scope.Dispose();
        GC.SuppressFinalize(this);
    }
}
