using Eurocentric.Features.AcceptanceTests.Utils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public sealed class WebAppFixture : InMemoryWebAppFixture
{
    private protected override string DbContainerName => TestCollection.Name;
}
