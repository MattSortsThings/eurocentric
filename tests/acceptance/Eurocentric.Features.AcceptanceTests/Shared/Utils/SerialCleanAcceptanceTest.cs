namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

[Category("acceptance")]
[NotInParallel("AcceptanceTests.Shared")]
public abstract class SerialCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync()
    {
        await SystemUnderTest.UnpauseDbContainerAsync();
        await SystemUnderTest.EraseAllDataAsync();
    }
}
