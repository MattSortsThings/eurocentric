using Eurocentric.Tests.Utils.Fixtures;

namespace Eurocentric.PublicApi.Tests.Subcutaneous.Utils;

public sealed class SeededWebAppFixture : WebAppFixture
{
    protected override async Task SeedDatabaseAsync() => await Task.Delay(250); // Imitate seeding database
}
