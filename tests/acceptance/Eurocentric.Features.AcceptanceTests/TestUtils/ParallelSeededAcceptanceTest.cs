using JetBrains.Annotations;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

[Category("acceptance")]
public abstract class ParallelSeededAcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required SeededWebAppFixture SystemUnderTest { get; [UsedImplicitly] init; }
}
