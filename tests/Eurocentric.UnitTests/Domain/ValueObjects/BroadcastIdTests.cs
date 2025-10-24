using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class BroadcastIdTests : UnitTest
{
    private static readonly Guid ArbitraryValue = Guid.Parse("e2c66954-6c82-4cf9-87ec-0fced2e078d5");

    [Test]
    public async Task Equals_should_return_0_when_other_is_instance()
    {
        // Arrange
        BroadcastId sut = BroadcastId.FromValue(ArbitraryValue);

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task Equals_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        Guid sutValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");
        Guid otherValue = Guid.Parse("99999999-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sutValue);
        BroadcastId other = BroadcastId.FromValue(otherValue);

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task Equals_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        Guid sutValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");
        Guid otherValue = Guid.Parse("00000000-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sutValue);
        BroadcastId other = BroadcastId.FromValue(otherValue);

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        BroadcastId sut = BroadcastId.FromValue(ArbitraryValue);

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        BroadcastId sut = BroadcastId.FromValue(ArbitraryValue);

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_BroadcastId_with_equal_Value()
    {
        // Arrange
        Guid sharedValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sharedValue);
        BroadcastId other = BroadcastId.FromValue(sharedValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_BroadcastId_with_unequal_Value()
    {
        // Arrange
        Guid sutValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");
        Guid otherValue = Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059");

        BroadcastId sut = BroadcastId.FromValue(sutValue);
        BroadcastId other = BroadcastId.FromValue(otherValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_BroadcastId()
    {
        // Arrange
        Guid sharedValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sharedValue);
        ContestId other = ContestId.FromValue(sharedValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        BroadcastId sut = BroadcastId.FromValue(ArbitraryValue);

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("26786949-965a-44cc-801a-22c6b5667a3b")]
    [Arguments("cbafd594-551e-4d87-8089-a8450d4ad059")]
    public async Task FromValue_should_return_instance_with_provided_value(string guidValue)
    {
        // Arrange
        Guid value = Guid.Parse(guidValue);

        // Act
        BroadcastId result = BroadcastId.FromValue(value);

        // Assert
        await Assert.That(result.Value).IsEqualTo(value);
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_BroadcastId_with_equal_Value()
    {
        // Arrange
        Guid sharedValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sharedValue);
        BroadcastId other = BroadcastId.FromValue(sharedValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_BroadcastId_with_unequal_Value()
    {
        // Arrange
        Guid sutValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");
        Guid otherValue = Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059");

        BroadcastId sut = BroadcastId.FromValue(sutValue);
        BroadcastId other = BroadcastId.FromValue(otherValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_BroadcastId()
    {
        // Arrange
        Guid sharedValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sharedValue);
        ContestId other = ContestId.FromValue(sharedValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_BroadcastId_with_equal_Value()
    {
        // Arrange
        Guid sharedValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sharedValue);
        BroadcastId other = BroadcastId.FromValue(sharedValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_BroadcastId_with_unequal_Value()
    {
        // Arrange
        Guid sutValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");
        Guid otherValue = Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059");

        BroadcastId sut = BroadcastId.FromValue(sutValue);
        BroadcastId other = BroadcastId.FromValue(otherValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_BroadcastId()
    {
        // Arrange
        Guid sharedValue = Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b");

        BroadcastId sut = BroadcastId.FromValue(sharedValue);
        ContestId other = ContestId.FromValue(sharedValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
