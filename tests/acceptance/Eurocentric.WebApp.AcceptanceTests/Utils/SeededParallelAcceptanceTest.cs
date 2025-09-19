namespace Eurocentric.WebApp.AcceptanceTests.Utils;

[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class SeededParallelAcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required SeededWebAppFixture SystemUnderTest { get; init; }
}
