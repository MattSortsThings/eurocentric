using Eurocentric.Tests.Unit.Utils;

namespace Eurocentric.Tests.Unit;

public sealed class PlaceholderUnitTests : UnitTests
{
    private static string GenerateString(int count) => string.Concat(Enumerable.Repeat("A", count));

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(99)]
    public async Task Should_always_pass(int count)
    {
        // Act
        string result = GenerateString(count);

        // Assert
        await Assert.That(result).Matches("A+");
    }
}
