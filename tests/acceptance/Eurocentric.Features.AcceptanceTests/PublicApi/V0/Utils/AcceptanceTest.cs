namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[Category("acceptance")]
public abstract class AcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.Keyed, Key = "PublicApi.V0")]
    public required SeededWebAppFixture SystemUnderTest { get; init; }
}
