namespace Eurocentric.AcceptanceTests.TestUtils;

[Category("container")]
[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class ParallelSeededAcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required WebAppFixture SystemUnderTest { get; init; }

    public sealed class SeededWebAppFixture : WebAppFixture
    {
        private protected override async Task SeedDbAsync() => await ExecuteScopedAsync(BackDoorOperations.SeedDbAsync);
    }
}
