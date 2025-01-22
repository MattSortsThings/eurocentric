using Eurocentric.Tests.Utils.Attributes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.Tests.Subcutaneous.Utils;

[DatabaseTest]
public abstract class SeededWebAppTests : IDisposable
{
    private readonly IServiceScope _scope;
    private SeededWebAppFixture? _fixture;

    protected SeededWebAppTests(SeededWebAppFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.Services.CreateScope();
        Sut = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    private protected ISender Sut { get; }

    public void Dispose()
    {
        _scope?.Dispose();
        _fixture = null;
        GC.SuppressFinalize(this);
    }
}
