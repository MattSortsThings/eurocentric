using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class ContestYearTests : UnitTest
{
    private const int ArbitraryValue = 2025;

    [Test]
    public async Task CompareTo_should_return_0_when_other_is_same_instance()
    {
        // Arrange
        ContestYear sut = ContestYear.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        const int sutValue = 2025;
        const int otherValue = 2050;

        ContestYear sut = ContestYear.FromValue(sutValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        const int sutValue = 2025;
        const int otherValue = 2016;

        ContestYear sut = ContestYear.FromValue(sutValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        ContestYear sut = ContestYear.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        ContestYear sut = ContestYear.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_ContestYear_with_equal_Value()
    {
        // Arrange
        const int sharedValue = 2025;

        ContestYear sut = ContestYear.FromValue(sharedValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_ContestYear_with_unequal_Value()
    {
        // Arrange
        const int sutValue = 2025;
        const int otherValue = 2050;

        ContestYear sut = ContestYear.FromValue(sutValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_ContestYear()
    {
        // Arrange
        const int sharedValue = 2025;

        ContestYear sut = ContestYear.FromValue(sharedValue).GetValueOrDefault();
        TestIntValue other = TestIntValue.FromValue(sharedValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        ContestYear sut = ContestYear.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(2016)]
    [Arguments(2025)]
    [Arguments(2050)]
    public async Task FromValue_should_return_ContestYear_with_provided_Value(int value)
    {
        // Act
        Result<ContestYear, IDomainError> result = ContestYear.FromValue(value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(actCode => actCode.Value, source => source.IsEqualTo(value));
    }

    [Test]
    public async Task FromValue_should_fail_given_int_value_less_than_2016()
    {
        // Arrange
        const int value = 2015;

        // Act
        Result<ContestYear, IDomainError> result = ContestYear.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal contest year value")
            .And.HasDetail("Contest year value must be an integer between 2016 and 2050.")
            .And.HasExtension("contestYear", value);
    }

    [Test]
    public async Task FromValue_should_fail_given_int_value_greater_than_2050()
    {
        // Arrange
        const int value = 2051;

        // Act
        Result<ContestYear, IDomainError> result = ContestYear.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal contest year value")
            .And.HasDetail("Contest year value must be an integer between 2016 and 2050.")
            .And.HasExtension("contestYear", value);
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_ContestYear_with_equal_Value()
    {
        // Arrange
        const int sharedValue = 2025;

        ContestYear sut = ContestYear.FromValue(sharedValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_ContestYear_with_unequal_Value()
    {
        // Arrange
        const int sutValue = 2025;
        const int otherValue = 2016;

        ContestYear sut = ContestYear.FromValue(sutValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_ContestYear()
    {
        // Arrange
        const int sharedValue = 2025;

        ContestYear sut = ContestYear.FromValue(sharedValue).GetValueOrDefault();
        TestIntValue other = TestIntValue.FromValue(sharedValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_ContestYear_with_equal_Value()
    {
        // Arrange
        const int sharedValue = 2025;

        ContestYear sut = ContestYear.FromValue(sharedValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_ContestYear_with_unequal_Value()
    {
        // Arrange
        const int sutValue = 2025;
        const int otherValue = 2016;

        ContestYear sut = ContestYear.FromValue(sutValue).GetValueOrDefault();
        ContestYear other = ContestYear.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_ContestYear()
    {
        // Arrange
        const int sharedValue = 2025;

        ContestYear sut = ContestYear.FromValue(sharedValue).GetValueOrDefault();
        TestIntValue other = TestIntValue.FromValue(sharedValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    private sealed class TestIntValue : Int32AtomicValueObject
    {
        private TestIntValue(int value)
            : base(value) { }

        public static TestIntValue FromValue(int value) => new(value);
    }
}
