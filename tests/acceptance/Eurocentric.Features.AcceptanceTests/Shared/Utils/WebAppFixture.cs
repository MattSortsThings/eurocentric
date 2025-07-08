using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.Shared.Utils;

public sealed class WebAppFixture : InMemoryWebAppFixture
{
    private protected override string DbContainerName => TestCollection.Name;
}
