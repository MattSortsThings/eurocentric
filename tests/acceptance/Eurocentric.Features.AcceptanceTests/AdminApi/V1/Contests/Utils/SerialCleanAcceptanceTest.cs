using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests.Utils;

[Category("acceptance")]
[NotInParallel("AcceptanceTests.AdminApi.V1.Contests")]
public abstract class SerialCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.Keyed, Key = "AcceptanceTests.AdminApi.V1.Contests")]
    public required CleanWebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.EraseAllDataAsync();
}
