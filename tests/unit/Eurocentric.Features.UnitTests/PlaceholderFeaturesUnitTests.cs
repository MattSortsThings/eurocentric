using Eurocentric.Features.UnitTests.Utils;

namespace Eurocentric.Features.UnitTests;

public static class PlaceholderFeaturesUnitTests
{
    public sealed class Placeholder : UnitTest
    {
        [Fact]
        public void Should_always_pass() => Assert.True(true);
    }
}
