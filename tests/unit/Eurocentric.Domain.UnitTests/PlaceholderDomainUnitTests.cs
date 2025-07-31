using Eurocentric.Domain.Enums;
using Eurocentric.Domain.UnitTests.Utils;

namespace Eurocentric.Domain.UnitTests;

public static class PlaceholderDomainUnitTests
{
    public sealed class Domain : UnitTest
    {
        [Test]
        public async Task Should_always_pass()
        {
            PointsValue[] result = Enum.GetValues<PointsValue>();

            await Assert.That(result).IsNotEmpty();
        }
    }
}
