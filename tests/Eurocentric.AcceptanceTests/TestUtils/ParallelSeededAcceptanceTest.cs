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
        private const string DboSeedingScriptPath = "Eurocentric.AcceptanceTests.TestUtils.Scripts.dbo_seeding.sql";

        private protected override async Task SeedDbAsync() =>
            await ExecuteScopedAsync(BackDoorOperations.ExecuteSqlFromScriptAsync(DboSeedingScriptPath));
    }
}
