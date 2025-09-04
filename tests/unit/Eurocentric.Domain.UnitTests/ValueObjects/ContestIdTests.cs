using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class ContestIdTests : UnitTest
{
    [Test]
    public async Task CompareTo_should_compare_by_Value()
    {
        // Arrange
        ContestId idA = ContestId.FromValue(Guid.Parse("aaaa8c81-dba0-4720-8ac9-3181cf9d26a1"));
        ContestId idB = ContestId.FromValue(Guid.Parse("bbbb8c81-dba0-4720-8ac9-3181cf9d26a1"));
        ContestId idC = ContestId.FromValue(Guid.Parse("cccc8c81-dba0-4720-8ac9-3181cf9d26a1"));
        ContestId idD = ContestId.FromValue(Guid.Parse("dddd8c81-dba0-4720-8ac9-3181cf9d26a1"));

        List<ContestId> sut = [idC, idB, idD, idA];

        // Act
        sut.Sort(Comparer<ContestId>.Default);

        // Assert
        await Assert.That(sut).IsEquivalentTo([idA, idB, idC, idD], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        Guid sutValue = Guid.Parse("01008c81-dba0-4720-8ac9-3181cf9d26a1");

        ContestId sut = ContestId.FromValue(sutValue);

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_Value_properties_are_equal()
    {
        // Arrange
        Guid sharedValue = Guid.Parse("01008c81-dba0-4720-8ac9-3181cf9d26a1");

        ContestId sut = ContestId.FromValue(sharedValue);
        ContestId other = ContestId.FromValue(sharedValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_Value_properties_are_unequal()
    {
        // Arrange
        Guid sutValue = Guid.Parse("01008c81-dba0-4720-8ac9-3181cf9d26a1");
        Guid otherValue = Guid.Parse("bdaaeb60-c632-46d4-9569-f94054474f3c");

        ContestId sut = ContestId.FromValue(sutValue);
        ContestId other = ContestId.FromValue(otherValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_given_null_other_arg()
    {
        // Arrange
        Guid sutValue = Guid.Parse("01008c81-dba0-4720-8ac9-3181cf9d26a1");

        ContestId sut = ContestId.FromValue(sutValue);

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("01008c81-dba0-4720-8ac9-3181cf9d26a1")]
    [Arguments("bdaaeb60-c632-46d4-9569-f94054474f3c")]
    public async Task FromValue_should_return_instance_with_value_arg(string guidValue)
    {
        // Arrange
        Guid value = Guid.Parse(guidValue);

        // Act
        ContestId result = ContestId.FromValue(value);

        // Assert
        await Assert.That(result.Value).IsEqualTo(value);
    }
}
