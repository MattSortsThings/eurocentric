using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;
using TUnit.Assertions.Enums;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class FinishingPositionTests : UnitTest
{
    private const int ArbitraryValue = 1;

    [Test]
    public async Task CompareTo_should_return_0_when_other_is_same_instance()
    {
        // Arrange
        FinishingPosition sut = FinishingPosition.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        const int sutValue = 2;
        const int otherValue = 3;

        FinishingPosition sut = FinishingPosition.FromValue(sutValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        const int sutValue = 2;
        const int otherValue = 1;

        FinishingPosition sut = FinishingPosition.FromValue(sutValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        FinishingPosition sut = FinishingPosition.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        FinishingPosition sut = FinishingPosition.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_FinishingPosition_with_equal_Value()
    {
        // Arrange
        const int sharedValue = 2;

        FinishingPosition sut = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_FinishingPosition_with_unequal_Value()
    {
        // Arrange
        const int sutValue = 2;
        const int otherValue = 3;

        FinishingPosition sut = FinishingPosition.FromValue(sutValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_FinishingPosition()
    {
        // Arrange
        const int sharedValue = 2;

        FinishingPosition sut = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();
        RunningOrderSpot other = RunningOrderSpot.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        FinishingPosition sut = FinishingPosition.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(1)]
    [Arguments(10)]
    [Arguments(26)]
    public async Task FromValue_should_return_FinishingPosition_with_provided_Value(int value)
    {
        // Act
        Result<FinishingPosition, IDomainError> result = FinishingPosition.FromValue(value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNotNull().And.HasValue(value);
    }

    [Test]
    public async Task FromValue_should_fail_given_int_value_less_than_1()
    {
        // Arrange
        const int value = 0;

        // Act
        Result<FinishingPosition, IDomainError> result = FinishingPosition.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal finishing position value")
            .And.HasDetail("Finishing position value must be an integer greater than or equal to 1.")
            .And.HasExtension("finishingPosition", value);
    }

    [Test]
    public async Task CreateSequence_should_return_requested_sequence_of_FinishingPosition_objects_scenario_1()
    {
        // Arrange
        const int count = 1;

        List<FinishingPosition> expected = [FinishingPosition.FromValue(1).GetValueOrDefault()];

        // Act
        FinishingPosition[] result = FinishingPosition.CreateSequence(count).ToArray();

        // Assert
        await Assert.That(result).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task CreateSequence_should_return_requested_sequence_of_FinishingPosition_objects_scenario_2()
    {
        // Arrange
        const int count = 4;

        List<FinishingPosition> expected =
        [
            FinishingPosition.FromValue(1).GetValueOrDefault(),
            FinishingPosition.FromValue(2).GetValueOrDefault(),
            FinishingPosition.FromValue(3).GetValueOrDefault(),
            FinishingPosition.FromValue(4).GetValueOrDefault(),
        ];

        // Act
        FinishingPosition[] result = FinishingPosition.CreateSequence(count).ToArray();

        // Assert
        await Assert.That(result).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task CreateSequence_should_return_empty_array_given_0_count_arg()
    {
        // Arrange
        const int count = 0;

        // Act
        FinishingPosition[] result = FinishingPosition.CreateSequence(count).ToArray();

        // Assert
        await Assert.That(result).IsEmpty();
    }

    [Test]
    public async Task CreateSequence_should_throw_given_negative_count_arg()
    {
        // Arrange
        const int count = -1;

        // Assert
        await Assert
            .That(() => FinishingPosition.CreateSequence(count))
            .Throws<ArgumentOutOfRangeException>()
            .WithMessage(
                """
                count ('-1') must be a non-negative value. (Parameter 'count')
                Actual value was -1.
                """
            );
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_FinishingPosition_with_equal_Value()
    {
        // Arrange
        const int sharedValue = 2;

        FinishingPosition sut = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_FinishingPosition_with_unequal_Value()
    {
        // Arrange
        const int sutValue = 2;
        const int otherValue = 3;

        FinishingPosition sut = FinishingPosition.FromValue(sutValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_FinishingPosition()
    {
        // Arrange
        const int sharedValue = 2;

        FinishingPosition sut = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();
        RunningOrderSpot other = RunningOrderSpot.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_FinishingPosition_with_equal_Value()
    {
        // Arrange
        const int sharedValue = 2;

        FinishingPosition sut = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_FinishingPosition_with_unequal_Value()
    {
        // Arrange
        const int sutValue = 2;
        const int otherValue = 3;

        FinishingPosition sut = FinishingPosition.FromValue(sutValue).GetValueOrDefault();
        FinishingPosition other = FinishingPosition.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_FinishingPosition()
    {
        // Arrange
        const int sharedValue = 2;

        FinishingPosition sut = FinishingPosition.FromValue(sharedValue).GetValueOrDefault();
        RunningOrderSpot other = RunningOrderSpot.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
