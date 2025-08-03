using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

[Category("acceptance")]
[NotInParallel(TestKeys.AdminApiV0)]
public abstract class SerialCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.EraseAllDataAsync();
}
