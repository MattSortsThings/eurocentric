using Eurocentric.AcceptanceTests.TestUtils.Contracts;
using Eurocentric.AcceptanceTests.TestUtils.Fixtures;

namespace Eurocentric.AcceptanceTests.TestUtils;

[Category("acceptance")]
[UseQualifiedDisplayName]
[ParallelLimiter<ParallelLimit>]
public abstract class AcceptanceTestBase
{
    [ClassDataSource<TestWebApp>(Shared = SharedType.PerClass)]
    public required ITestWebApp SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetTestWebAppAsync() => await SystemUnderTest.ResetAsync();
}
