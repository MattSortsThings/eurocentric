using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

[Category("acceptance")]
[NotInParallel("AcceptanceTests.AdminApi.V0")]
public abstract class CleanSerialAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.ExecuteScopedAsync(BackdoorOperations.EraseAllDataAsync);
}
