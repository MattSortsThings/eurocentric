namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class ParallelDbOfflineAcceptanceTest
{
    [ClassDataSource<DbOfflineWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required DbOfflineWebAppFixture SystemUnderTest { get; init; }
}
