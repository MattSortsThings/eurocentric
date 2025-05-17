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

    private protected abstract int ApiMajorVersion { get; }

    private protected abstract int ApiMinorVersion { get; }

    public void Dispose()
    {
        Sut.Reset();
        GC.SuppressFinalize(this);
    }

    private protected AdminApiV1Driver CreateAdminApiV1Driver() =>
        AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion);
}
