namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    [ClassDataSource<DbContainerFixture>(Shared = SharedType.PerClass)]
    public override required DbContainerFixture DbContainerFixture { get; init; }
}
