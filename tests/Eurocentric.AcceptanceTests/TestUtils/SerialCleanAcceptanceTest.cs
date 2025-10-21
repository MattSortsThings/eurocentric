namespace Eurocentric.AcceptanceTests.TestUtils;

[Category("container")]
[Category("acceptance")]
[NotInParallel("serial")]
public abstract class SerialCleanAcceptanceTest
{
    [ClassDataSource<CleanWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required WebAppFixture SystemUnderTest { get; init; }

    [After(Test)]
    public async Task TeardownAsync() => await SystemUnderTest.ExecuteScopedAsync(BackDoorOperations.ResetDbAsync);

    public sealed class CleanWebAppFixture : WebAppFixture
    {
        private protected override Task SeedDbAsync() => Task.CompletedTask;
    }
}
