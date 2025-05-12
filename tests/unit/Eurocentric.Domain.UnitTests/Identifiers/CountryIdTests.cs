using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.TestUtils;

namespace Eurocentric.Domain.UnitTests.Identifiers;

public sealed class CountryIdTests : UnitTestBase
{
    public sealed class EqualsMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            Guid sutValue = Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57");

            ContestId sut = ContestId.FromValue(sutValue);

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            Guid sharedValue = Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57");

            ContestId sut = ContestId.FromValue(sharedValue);
            ContestId other = ContestId.FromValue(sharedValue);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_values()
        {
            // Arrange
            Guid sutValue = Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57");
            Guid otherValue = Guid.Parse("acee013d-5d26-4cf8-87ac-d21cbe6eb5d7");

            ContestId sut = ContestId.FromValue(sutValue);
            ContestId other = ContestId.FromValue(otherValue);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            Guid sutValue = Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57");

            ContestId sut = ContestId.FromValue(sutValue);

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }

    public sealed class FromValueStaticMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_value_arg_as_value_property()
        {
            // Arrange
            Guid idValue = Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57");

            // Act
            CountryId result = CountryId.FromValue(idValue);

            // Assert
            Assert.Equal(idValue, result.Value);
        }
    }
}
