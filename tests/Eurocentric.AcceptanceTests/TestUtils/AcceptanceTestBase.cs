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
    /// <summary>
    ///     A test-specific web app using its own isolated test database, which is created in an empty state before the test
    ///     starts and disposed of after the test completes.
    /// </summary>
    [ClassDataSource<TestWebApp>(Shared = SharedType.None)]
    public required ITestWebApp SystemUnderTest { get; init; }
}
