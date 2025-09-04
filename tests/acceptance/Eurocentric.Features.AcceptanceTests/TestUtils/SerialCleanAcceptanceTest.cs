using JetBrains.Annotations;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

[Category("acceptance")]
[NotInParallel(nameof(SerialCleanAcceptanceTest))]
public abstract class SerialCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required CleanWebAppFixture SystemUnderTest { get; [UsedImplicitly] init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.ExecuteScopedAsync(BackdoorOperations.EraseAllDataAsync);
}
