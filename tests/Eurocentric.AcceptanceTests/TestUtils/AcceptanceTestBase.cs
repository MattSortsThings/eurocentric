using Eurocentric.AcceptanceTests.TestUtils.Fixtures;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Abstract base class for all acceptance tests.
/// </summary>
[Category("acceptance")]
[UseQualifiedDisplayName]
[ParallelLimiter<ParallelLimit>]
public abstract class AcceptanceTestBase
{
    [ClassDataSource<TestWebApp>(Shared = SharedType.None)]
    public required ITestWebApp SystemUnderTest { get; init; }
}
