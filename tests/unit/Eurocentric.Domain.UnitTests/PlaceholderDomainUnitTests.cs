using Eurocentric.Domain.UnitTests.Utils;

namespace Eurocentric.Domain.UnitTests;

public static class PlaceholderDomainUnitTests
{
    public sealed class Placeholder : UnitTest
    {
        [Fact]
        public void Should_always_pass() => Assert.True(true);
    }
}
