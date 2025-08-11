using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.Utils;

[Category("acceptance")]
[NotInParallel("AcceptanceTests.AdminApi.V1.Broadcasts")]
public abstract class SerialCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.Keyed, Key = "AcceptanceTests.AdminApi.V1.Broadcasts")]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.EraseAllDataAsync();
}
