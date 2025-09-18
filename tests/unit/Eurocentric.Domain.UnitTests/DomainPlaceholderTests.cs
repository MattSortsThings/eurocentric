using Eurocentric.Domain.Enums;
using Eurocentric.Domain.UnitTests.Utils;

namespace Eurocentric.Domain.UnitTests;

[Category("placeholder")]
public sealed class DomainPlaceholderTests : UnitTest
{
    [Test]
    public async Task Should_always_pass()
    {
        ContestStage[] values = Enum.GetValues<ContestStage>();

        await Assert.That(values).HasCount(3);
    }
}
