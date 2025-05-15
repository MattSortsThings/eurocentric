namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

[Trait("Category", "container")]
[Trait("Category", "acceptance")]
[Collection(nameof(AdminApiV1TestCollection))]
public abstract class AcceptanceTestBase : IDisposable
{
    protected AcceptanceTestBase(WebAppFixture webAppFixture)
    {
        Sut = webAppFixture;
    }

    private protected WebAppFixture Sut { get; }

    public void Dispose()
    {
        Sut.Reset();
        GC.SuppressFinalize(this);
    }

    private protected abstract AdminApiV1Driver CreateAdminApiV1Driver();
}
