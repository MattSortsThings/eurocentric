using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    [ClassDataSource<DbContainerFixture>(Shared = SharedType.PerClass)]
    public override required DbContainerFixture DbContainerFixture { get; init; }
}
