namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.TestUtils;

[Category("acceptance")]
public abstract class SeededParallelAcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required SeededWebAppFixture SystemUnderTest { get; init; }
}
