using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class TelevoteAwardTests : UnitTestBase
{
    public sealed class Constructor : UnitTestBase
    {
        [Fact]
        public void Should_throw_exception_when_votingCountryId_arg_is_null()
        {
            // Act
            Action act = () => _ = new TelevoteAward(null!, PointsValue.Zero);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'votingCountryId')", exception.Message);
        }
    }

    public sealed class EqualsMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("d879798c-d3a9-4393-9282-1d82b34c6dc2"));
            const PointsValue sutPointsValue = PointsValue.One;

            TelevoteAward sut = new(sutVotingCountryId, sutPointsValue);

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("d879798c-d3a9-4393-9282-1d82b34c6dc2"));
            const PointsValue sharedPointsValue = PointsValue.One;

            TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
            TelevoteAward other = new(sharedVotingCountryId, sharedPointsValue);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_VotingCountryId_values()
        {
            // Arrange
            CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("d879798c-d3a9-4393-9282-1d82b34c6dc2"));
            CountryId otherVotingCountryId = CountryId.FromValue(Guid.Parse("8a5a6c91-2c76-4287-9523-2c576100e0c4"));
            const PointsValue sharedPointsValue = PointsValue.One;

            TelevoteAward sut = new(sutVotingCountryId, sharedPointsValue);
            TelevoteAward other = new(otherVotingCountryId, sharedPointsValue);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_PointsValue_values()
        {
            // Arrange
            CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("d879798c-d3a9-4393-9282-1d82b34c6dc2"));
            const PointsValue sutPointsValue = PointsValue.Zero;
            const PointsValue otherPointsValue = PointsValue.Twelve;

            TelevoteAward sut = new(sharedVotingCountryId, sutPointsValue);
            TelevoteAward other = new(sharedVotingCountryId, otherPointsValue);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_given_null_value_arg()
        {
            // Arrange
            CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("d879798c-d3a9-4393-9282-1d82b34c6dc2"));
            const PointsValue sutPointsValue = PointsValue.One;

            TelevoteAward sut = new(sutVotingCountryId, sutPointsValue);

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }
}
