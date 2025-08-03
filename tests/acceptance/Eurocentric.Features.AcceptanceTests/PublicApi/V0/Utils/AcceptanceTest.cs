using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[Category("acceptance")]
public abstract class AcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.Keyed, Key = TestKeys.PublicApiV0)]
    public required SeededWebAppFixture SystemUnderTest { get; init; }
}
