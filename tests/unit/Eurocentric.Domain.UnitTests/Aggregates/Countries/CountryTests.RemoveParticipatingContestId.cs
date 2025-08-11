using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Aggregates.Countries;

public sealed partial class CountryTests
{
    [Test]
    public async Task RemoveParticipatingContestId_should_remove_equal_ContestId()
    {
        // Arrange
        Country sut = CreateCountry();

        ContestId contestId = ContestId.FromValue(Guid.Parse("905d7fcc-87b7-46e6-a5e0-f5615400ccd6"));

        sut.AddParticipatingContestId(contestId);

        // Assert
        await Assert.That(sut.ParticipatingContestIds).HasSingleItem();

        // Act
        sut.RemoveParticipatingContestId(contestId);

        // Assert
        await Assert.That(sut.ParticipatingContestIds).IsEmpty();
    }

    [Test]
    public async Task RemoveParticipatingContestId_should_throw_given_null_contestId_arg()
    {
        // Arrange
        Country sut = CreateCountry();

        Action act = () => sut.RemoveParticipatingContestId(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'contestId')")
            .WithParameterName("contestId");
    }

    [Test]
    public async Task RemoveParticipatingContestId_should_throw_given_contestId_arg_matching_no_ParticipatingContestId()
    {
        // Arrange
        Country sut = CreateCountry();

        ContestId contestId = ContestId.FromValue(Guid.Parse("905d7fcc-87b7-46e6-a5e0-f5615400ccd6"));

        Action act = () => sut.RemoveParticipatingContestId(contestId);

        // Assert
        await Assert.That(act).Throws<ArgumentException>()
            .WithMessage("Country ParticipatingContestIds collection does not contain provided ContestId value.");
    }
}
