using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;

[CollectionDefinition(Name, DisableParallelization = false)]
public sealed class TestCollection : ICollectionFixture<WebAppFixture>
{
    public const string Name = "AcceptanceTests.AdminApi.V1";
}
