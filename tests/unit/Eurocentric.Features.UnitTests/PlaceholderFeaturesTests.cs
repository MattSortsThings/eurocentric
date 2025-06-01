using Eurocentric.Features.UnitTests.Utilities;

namespace Eurocentric.Features.UnitTests;

[Trait("Category", "placeholder")]
public sealed class PlaceholderFeaturesTests : UnitTestBase
{
    [Fact]
    public void Should_always_pass() => Assert.True(true);
}
