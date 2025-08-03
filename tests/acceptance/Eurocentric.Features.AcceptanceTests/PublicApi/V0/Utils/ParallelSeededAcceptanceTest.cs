namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[Category("acceptance")]
public abstract class ParallelSeededAcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required SeededWebAppFixture SystemUnderTest { get; init; }
}
