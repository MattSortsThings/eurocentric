using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests;

[Category("placeholder")]
public sealed class PlaceholderUnitTests : UnitTest
{
    [Test]
    [Arguments(2, 3)]
    [Arguments(3, 4)]
    [Arguments(4, 5)]
    [Arguments(5, 6)]
    public async Task Should_always_pass(int x, int y)
    {
        // Act
        int result = x * y;

        // Assert
        await Assert.That(result).IsPositive();
    }
}
