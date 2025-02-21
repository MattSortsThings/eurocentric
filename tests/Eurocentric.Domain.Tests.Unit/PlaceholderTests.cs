using Eurocentric.Domain.Tests.Unit.Utils;

namespace Eurocentric.Domain.Tests.Unit;

public static class PlaceholderTests
{
    public sealed class DomainPlaceholder : UnitTest
    {
        [Fact]
        public void Should_always_pass() => Assert.True(true);
    }
}
