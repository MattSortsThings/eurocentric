using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.UnitTests.Utils;

namespace Eurocentric.Features.UnitTests;

public static class PlaceholderFeaturesUnitTests
{
    public sealed class Features : UnitTest
    {
        [Test]
        public async Task Should_always_pass()
        {
            QueryableContestStage[] result = Enum.GetValues<QueryableContestStage>();

            await Assert.That(result).IsNotEmpty();
        }
    }
}
