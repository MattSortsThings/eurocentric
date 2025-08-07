using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.AssertConditions.Throws;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class TelevoteAwardTests : UnitTest
{
    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("30227509-467a-46dd-af6a-cd4568508f90"));
        const PointsValue sutPointsValue = PointsValue.Ten;

        TelevoteAward sut = new(sutVotingCountryId, sutPointsValue);

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_VotingCountryId_and_PointsValue_properties_are_equal()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("30227509-467a-46dd-af6a-cd4568508f90"));
        const PointsValue sharedPointsValue = PointsValue.Ten;

        TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_VotingCountryId_properties_are_unequal()
    {
        // Arrange
        CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("30227509-467a-46dd-af6a-cd4568508f90"));
        CountryId otherVotingCountryId = CountryId.FromValue(Guid.Parse("e165cb68-4fa9-4185-87d8-d70ea63f46c8"));

        const PointsValue sharedPointsValue = PointsValue.Ten;

        TelevoteAward sut = new(sutVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(otherVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_PointsValue_properties_are_unequal()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("30227509-467a-46dd-af6a-cd4568508f90"));

        const PointsValue sutPointsValue = PointsValue.Ten;
        const PointsValue otherPointsValue = PointsValue.Zero;

        TelevoteAward sut = new(sharedVotingCountryId, sutPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, otherPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_given_null_other_arg()
    {
        // Arrange
        CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("30227509-467a-46dd-af6a-cd4568508f90"));
        const PointsValue sutPointsValue = PointsValue.Ten;

        TelevoteAward sut = new(sutVotingCountryId, sutPointsValue);

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Constructor_should_initialize_from_votingCountryId_and_pointsValue_args()
    {
        // Arrange
        CountryId votingCountryId = CountryId.FromValue(Guid.Parse("30227509-467a-46dd-af6a-cd4568508f90"));
        const PointsValue pointsValue = PointsValue.Ten;

        // Act
        TelevoteAward result = new(votingCountryId, pointsValue);

        // Assert
        await Assert.That(result.VotingCountryId).IsEqualTo(votingCountryId);

        await Assert.That(result.PointsValue).IsEqualTo(pointsValue);
    }

    [Test]
    public async Task Constructor_should_throw_given_null_votingCountryId_arg()
    {
        // Arrange
        const PointsValue arbitraryPointsValue = default;

        Action act = () => _ = new TelevoteAward(null!, arbitraryPointsValue);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'votingCountryId')")
            .WithParameterName("votingCountryId");
    }
}
