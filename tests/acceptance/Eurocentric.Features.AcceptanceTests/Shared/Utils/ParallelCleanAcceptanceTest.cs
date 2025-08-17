namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class ParallelCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required CleanWebAppFixture SystemUnderTest { get; init; }
}
