using Eurocentric.Infrastructure.UnitTests.Utils;

namespace Eurocentric.Infrastructure.UnitTests;

[Category("placeholder")]
public sealed class InfrastructurePlaceholderTests : UnitTest
{
    [Test]
    [Arguments(4, 5)]
    [Arguments(2, 10)]
    public async Task Should_always_pass(int x, int y)
    {
        int result = x * y;

        await Assert.That(result).IsPositive();
    }
}
