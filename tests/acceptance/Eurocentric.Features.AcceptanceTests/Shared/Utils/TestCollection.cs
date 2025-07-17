using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

[CollectionDefinition(Name, DisableParallelization = false)]
public sealed class TestCollection : ICollectionFixture<WebAppFixture>
{
    public const string Name = "AcceptanceTests.Shared";
}
