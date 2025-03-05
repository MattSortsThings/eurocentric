using Eurocentric.Domain.BaseTypes;
using Eurocentric.Domain.Tests.Unit.Utils;
using Eurocentric.Domain.Tests.Unit.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Tests.Unit.BaseTypes;

public static class AggregateRootIdComparerTests
{
    private sealed class TestCountry : AggregateRoot<CountryId>
    {
        public TestCountry(CountryId id) : base(id) { }
    }

    public sealed class EqualsMethod : UnitTest
    {
        private static readonly Guid ContestIdA = Guid.Parse("2a0fc598-a1ba-4976-a5af-33ac8c56f72e");
        private static readonly Guid ContestIdB = Guid.Parse("fd0844d2-208b-4068-8f4b-8eebbfe8fa7e");

        [Fact]
        public void Should_return_true_when_both_args_are_same_instance()
        {
            // Arrange
            TestCountry country = new(CountryId.FromValue(ContestIdA));

            // Act
            bool result = AggregateRoot<CountryId>.IdComparer.Equals(country, country);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_true_when_args_are_different_instance_with_equal_ID_values()
        {
            // Arrange
            TestCountry firstCountry = new(CountryId.FromValue(ContestIdA));
            TestCountry secondCountry = new(CountryId.FromValue(ContestIdA));

            // Act
            bool result = AggregateRoot<CountryId>.IdComparer.Equals(firstCountry, secondCountry);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_true_when_both_args_are_null()
        {
            // Act
            bool result = AggregateRoot<CountryId>.IdComparer.Equals(null, null);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_false_when_args_are_different_instance_with_unequal_ID_values()
        {
            // Arrange
            TestCountry firstCountry = new(CountryId.FromValue(ContestIdA));
            TestCountry secondCountry = new(CountryId.FromValue(ContestIdB));

            // Act
            bool result = AggregateRoot<CountryId>.IdComparer.Equals(firstCountry, secondCountry);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_return_false_when_first_arg_is_null_and_second_arg_is_not_null()
        {
            // Arrange
            TestCountry country = new(CountryId.FromValue(ContestIdA));

            // Act
            bool result = AggregateRoot<CountryId>.IdComparer.Equals(null, country);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_return_false_when_first_arg_is_not_null_and_second_arg_is_null()
        {
            // Arrange
            TestCountry country = new(CountryId.FromValue(ContestIdA));

            // Act
            bool result = AggregateRoot<CountryId>.IdComparer.Equals(country, null);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
