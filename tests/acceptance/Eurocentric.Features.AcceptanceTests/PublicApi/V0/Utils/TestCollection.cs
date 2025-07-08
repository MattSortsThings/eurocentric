namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class TestCollection : ICollectionFixture<WebAppFixture>
{
    public const string Name = "acceptance-tests-public-api-v0";
}
