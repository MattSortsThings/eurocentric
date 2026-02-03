using Eurocentric.Tests.Acceptance.Utils.Contracts;
using Eurocentric.Tests.Acceptance.Utils.Execution;
using Eurocentric.Tests.Acceptance.Utils.Fixtures;

namespace Eurocentric.Tests.Acceptance.Utils;

[QualifiedDisplayName]
[Category("acceptance")]
[ParallelLimiter<ParallelLimit>]
public abstract class AcceptanceTests
{
    [ClassDataSource<TestWebApp>(Shared = SharedType.PerClass)]
    public required ITestWebApp SystemUnderTest { get; init; }

    [After(Test)]
    public async Task ResetAsync() => await SystemUnderTest.ResetAsync();
}
