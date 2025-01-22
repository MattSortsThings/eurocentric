using Eurocentric.Tests.Utils.Attributes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.Tests.Subcutaneous.Utils;

[DatabaseTest]
public abstract class SeededWebAppTests : IDisposable
{
    private readonly SeededWebAppFixture _fixture;
    private readonly IServiceScope _scope;

    protected SeededWebAppTests(SeededWebAppFixture fixture)
    {
        _fixture = fixture;
        _scope = fixture.Services.CreateScope();
        Sut = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    private protected ISender Sut { get; }

    public void Dispose()
    {
        _scope.Dispose();
        GC.SuppressFinalize(this);
    }
}
