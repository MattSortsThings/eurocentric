namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

[CollectionDefinition(Name, DisableParallelization = false)]
public sealed class TestCollection : ICollectionFixture<WebAppFixture>
{
    public const string Name = "acceptance-tests-shared";
}
