using Eurocentric.Tests.Utils.Attributes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi.Tests.Subcutaneous.Utils;

[DatabaseTest]
[Collection(nameof(CleanWebAppTestCollection))]
public abstract class CleanWebAppTests : IDisposable
{
    private readonly IServiceScope _scope;
    private CleanWebAppFixture? _fixture;

    protected CleanWebAppTests(CleanWebAppFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.Services.CreateScope();
        Sut = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    private protected ISender Sut { get; }

    public void Dispose()
    {
        _scope?.Dispose();
        _fixture?.Reset();
        _fixture = null;
        GC.SuppressFinalize(this);
    }
}
