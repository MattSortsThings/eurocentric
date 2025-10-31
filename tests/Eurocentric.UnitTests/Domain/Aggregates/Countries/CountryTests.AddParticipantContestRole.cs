using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.Countries;

public sealed partial class CountryTests
{
    [Test]
    public async Task AddParticipantContestRole_should_add_ContestRole()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId contestId = ContestId.FromValue(Guid.Parse("da143967-afc4-498f-955d-5b3341f91f40"));

        // Assert
        await Assert.That(sut.ContestRoles).IsEmpty();

        // Act
        sut.AddParticipantContestRole(contestId);

        // Assert
        ContestRole singleContestRole = await Assert.That(sut.ContestRoles).HasSingleItem();

        await Assert
            .That(singleContestRole)
            .HasProperty(role => role.ContestId, contestId)
            .And.HasProperty(role => role.ContestRoleType, ContestRoleType.Participant);
    }

    [Test]
    public async Task AddParticipantContestRole_should_throw_given_null_contestId_arg()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId nullContestId = null!;

        // Assert
        await Assert
            .That(() => sut.AddParticipantContestRole(nullContestId))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'contestId')");

        await Assert.That(sut.ContestRoles).IsEmpty();
    }

    [Test]
    public async Task AddParticipantContestRole_should_throw_on_contest_ID_conflict_scenario_1()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId contestId = ContestId.FromValue(Guid.Parse("da143967-afc4-498f-955d-5b3341f91f40"));

        sut.AddParticipantContestRole(contestId);

        // Assert
        await Assert.That(sut.ContestRoles).HasSingleItem();

        // Assert
        await Assert
            .That(() => sut.AddParticipantContestRole(contestId))
            .Throws<ArgumentException>()
            .WithMessage("Country already has a ContestRole with the provided ContestId.");

        await Assert.That(sut.ContestRoles).HasSingleItem();
    }

    [Test]
    public async Task AddParticipantContestRole_should_throw_on_contest_ID_conflict_scenario_2()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId contestId = ContestId.FromValue(Guid.Parse("da143967-afc4-498f-955d-5b3341f91f40"));

        sut.AddGlobalTelevoteContestRole(contestId);

        // Assert
        await Assert.That(sut.ContestRoles).HasSingleItem();

        // Assert
        await Assert
            .That(() => sut.AddParticipantContestRole(contestId))
            .Throws<ArgumentException>()
            .WithMessage("Country already has a ContestRole with the provided ContestId.");

        await Assert.That(sut.ContestRoles).HasSingleItem();
    }
}
