using Eurocentric.Domain.Placeholders;
using Eurocentric.Domain.UnitTests.Utils;

namespace Eurocentric.Domain.UnitTests;

public sealed class PlaceholderTests : UnitTestBase
{
    [Trait("Category", "Placeholder")]
    public sealed class LineEnum : UnitTestBase
    {
        [Fact]
        public void Should_have_3_names()
        {
            // Act
            string[] names = Enum.GetNames<Line>();

            // Assert
            Assert.Equal(3, names.Length);
        }
    }
}
