using Eurocentric.Domain.Tests.Unit.Utils;

namespace Eurocentric.Domain.Tests.Unit;

public static class PlaceholderDomainUnitTests
{
    public sealed class Placeholder : UnitTests
    {
        [Fact]
        public void Should_always_pass() => Assert.True(true);
    }
}
