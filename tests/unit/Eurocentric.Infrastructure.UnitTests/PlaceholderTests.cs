using Eurocentric.Infrastructure.UnitTests.Utils;

namespace Eurocentric.Infrastructure.UnitTests;

[Trait("Category", "Placeholder")]
public sealed class PlaceholderTests : UnitTestBase
{
    [Fact]
    public void Should_always_pass() => Assert.True(true);
}
