namespace Eurocentric.Tests.Acceptance.Utils;

[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class AcceptanceTests
{
    /// <summary>
    ///     A Microsoft SQL Server instance running inside a container.
    /// </summary>
    [ClassDataSource<MsSqlServerFixture>(Shared = SharedType.PerTestSession)]
    public required MsSqlServerFixture DbServer { get; init; }
}
