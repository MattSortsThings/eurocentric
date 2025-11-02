using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.Countries;

public sealed partial class CountryTests
{
    [Test]
    public async Task RemoveContestRole_should_remove_ContestRole_matching_contestId_arg_scenario_1()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId contestId = ContestId.FromValue(Guid.Parse("a76731b6-30bd-4148-866e-5a43364b413d"));

        sut.AddParticipantContestRole(contestId);

        // Assert
        await Assert
            .That(sut.ContestRoles)
            .HasSingleItem()
            .And.Contains(contestRole =>
                contestRole.ContestId.Equals(contestId) && contestRole.ContestRoleType == ContestRoleType.Participant
            );

        // Act
        sut.RemoveContestRole(contestId);

        // Assert
        await Assert.That(sut.ContestRoles).IsEmpty();
    }

    [Test]
    public async Task RemoveContestRole_should_remove_ContestRole_matching_contestId_arg_scenario_2()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId contestId1 = ContestId.FromValue(Guid.Parse("a76731b6-30bd-4148-866e-5a43364b413d"));
        ContestId contestId2 = ContestId.FromValue(Guid.Parse("f276d02d-fb00-4423-8f46-42d885c84763"));

        sut.AddParticipantContestRole(contestId1);
        sut.AddGlobalTelevoteContestRole(contestId2);

        // Assert
        await Assert
            .That(sut.ContestRoles)
            .HasCount(2)
            .And.Contains(contestRole =>
                contestRole.ContestId.Equals(contestId1) && contestRole.ContestRoleType == ContestRoleType.Participant
            )
            .And.Contains(contestRole =>
                contestRole.ContestId.Equals(contestId2)
                && contestRole.ContestRoleType == ContestRoleType.GlobalTelevote
            );

        // Act
        sut.RemoveContestRole(contestId1);

        // Assert
        await Assert
            .That(sut.ContestRoles)
            .HasSingleItem()
            .And.Contains(contestRole =>
                contestRole.ContestId.Equals(contestId2)
                && contestRole.ContestRoleType == ContestRoleType.GlobalTelevote
            );
    }

    [Test]
    public async Task RemoveContestRole_should_remove_ContestRole_matching_contestId_arg_scenario_3()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId contestId1 = ContestId.FromValue(Guid.Parse("a76731b6-30bd-4148-866e-5a43364b413d"));
        ContestId contestId2 = ContestId.FromValue(Guid.Parse("f276d02d-fb00-4423-8f46-42d885c84763"));

        sut.AddParticipantContestRole(contestId1);
        sut.AddGlobalTelevoteContestRole(contestId2);

        // Assert
        await Assert
            .That(sut.ContestRoles)
            .HasCount(2)
            .And.Contains(contestRole =>
                contestRole.ContestId.Equals(contestId1) && contestRole.ContestRoleType == ContestRoleType.Participant
            )
            .And.Contains(contestRole =>
                contestRole.ContestId.Equals(contestId2)
                && contestRole.ContestRoleType == ContestRoleType.GlobalTelevote
            );

        // Act
        sut.RemoveContestRole(contestId2);

        // Assert
        await Assert
            .That(sut.ContestRoles)
            .HasSingleItem()
            .And.Contains(contestRole =>
                contestRole.ContestId.Equals(contestId1) && contestRole.ContestRoleType == ContestRoleType.Participant
            );
    }

    [Test]
    public async Task RemoveContestRole_should_throw_given_null_contestId_arg()
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
            .That(() => sut.RemoveContestRole(nullContestId))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'contestId')");
    }

    [Test]
    public async Task RemoveContestRole_should_throw_given_contestId_arg_matching_no_ContestRole()
    {
        // Arrange
        Country sut = Country
            .Create()
            .WithCountryCode(DefaultCountryCode)
            .WithCountryName(DefaultCountryName)
            .Build(() => DefaultCountryId)
            .GetValueOrDefault();

        ContestId orphanContestId = ContestId.FromValue(Guid.Parse("a76731b6-30bd-4148-866e-5a43364b413d"));

        // Assert
        await Assert
            .That(() => sut.RemoveContestRole(orphanContestId))
            .Throws<ArgumentException>()
            .WithMessage("Country has no ContestRole with the provided ContestId.");
    }
}
