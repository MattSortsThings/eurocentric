using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.UnitTests.TestUtils;

namespace Eurocentric.Features.UnitTests;

[Category("placeholder")]
public sealed class PlaceholderFeaturesUnitTests : UnitTest
{
    [Test]
    public async Task QueryableContestStage_enum_should_have_5_values()
    {
        // Act
        QueryableContestStage[] result = Enum.GetValues<QueryableContestStage>();

        // Assert
        await Assert.That(result).HasCount(5);
    }
}
