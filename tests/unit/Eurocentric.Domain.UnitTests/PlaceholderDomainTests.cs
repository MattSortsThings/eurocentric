using Eurocentric.Domain.UnitTests.Utilities;

namespace Eurocentric.Domain.UnitTests;

[Trait("Category", "placeholder")]
public sealed class PlaceholderDomainTests : UnitTestBase
{
    [Fact]
    public void Should_always_pass() => Assert.True(true);
}
