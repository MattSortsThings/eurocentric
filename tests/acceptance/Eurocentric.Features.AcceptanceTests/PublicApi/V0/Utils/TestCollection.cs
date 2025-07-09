using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[CollectionDefinition(Name, DisableParallelization = false)]
public sealed class TestCollection : ICollectionFixture<WebAppFixture>
{
    public const string Name = "acceptance-tests-public-api-v0";
}
