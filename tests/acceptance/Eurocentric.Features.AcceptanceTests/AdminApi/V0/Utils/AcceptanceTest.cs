namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

[Category("acceptance")]
[NotInParallel("AdminApi.V0")]
public abstract class AcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.Keyed, Key = "AdminApi.V0")]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.EraseAllDataAsync();
}
