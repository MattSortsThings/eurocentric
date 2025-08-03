namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

[NotInParallel("Shared")]
[Category("acceptance")]
public abstract class SerialCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.UnpauseDbContainerAsync();
}
