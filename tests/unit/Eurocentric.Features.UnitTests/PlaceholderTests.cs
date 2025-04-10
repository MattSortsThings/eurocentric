using Eurocentric.Features.UnitTests.Utils;

namespace Eurocentric.Features.UnitTests;

[Trait("Category", "Placeholder")]
public sealed class PlaceholderTests : UnitTestBase
{
    [Fact]
    public void Should_always_pass() => Assert.True(true);
}
