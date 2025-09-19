namespace Eurocentric.WebApp.AcceptanceTests.Utils;

[Category("acceptance")]
[NotInParallel("serial")]
public abstract class CleanSerialAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task TeardownAsync() => await SystemUnderTest.EraseAllDataAsync();
}
