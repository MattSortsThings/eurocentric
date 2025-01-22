using Eurocentric.Domain.Queries.Common;
using Eurocentric.Tests.Utils.Attributes;

namespace Eurocentric.Domain.Tests.Unit;

public static class VotingMethodTests
{
    [PlaceholderTest]
    public sealed class ToStringMethod
    {
        [Fact]
        public void Should_return_enum_member_name()
        {
            // Act
            string? result = VotingMethod.Jury.ToString();

            // Assert
            Assert.Equal("Jury", result);
        }
    }
}
