using Eurocentric.Apis.Public.UnitTests.Utils;
using Eurocentric.Apis.Public.V0.Enums;

namespace Eurocentric.Apis.Public.UnitTests;

[Category("placeholder")]
public sealed class PublicApiPlaceholderTests : UnitTest
{
    [Test]
    public async Task Should_always_pass()
    {
        ContestStage[] values = Enum.GetValues<ContestStage>();

        await Assert.That(values).HasCount(3);
    }
}
