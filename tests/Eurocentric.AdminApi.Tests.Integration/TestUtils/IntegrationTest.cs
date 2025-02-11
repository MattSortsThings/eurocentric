using Eurocentric.TestUtils.Categories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi.Tests.Integration.TestUtils;

[IntegrationTest]
[Collection(nameof(CleanWebAppFixtureTestCollection))]
public abstract class IntegrationTest : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly CleanWebAppFixture _webAppFixture;

    protected IntegrationTest(CleanWebAppFixture webAppFixture)
    {
        _webAppFixture = webAppFixture;
        _scope = webAppFixture.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    private protected ISender Sender { get; }

    public void Dispose()
    {
        _webAppFixture.Reset();
        _scope.Dispose();
        GC.SuppressFinalize(this);
    }
}
