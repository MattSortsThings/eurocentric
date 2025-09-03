using Eurocentric.Domain.Enums;
using Eurocentric.Domain.UnitTests.TestUtils;

namespace Eurocentric.Domain.UnitTests;

[Category("placeholder")]
public sealed class PlaceholderDomainUnitTests : UnitTest
{
    [Test]
    public async Task PointsValue_enum_should_have_11_values()
    {
        // Act
        PointsValue[] result = Enum.GetValues<PointsValue>();

        // Assert
        await Assert.That(result).HasCount(11);
    }
}
