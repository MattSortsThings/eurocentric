using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Aggregates.Countries;

public sealed partial class CountryTests
{
    [Test]
    public async Task AddParticipatingContestId_should_add_copy_of_ContestId()
    {
        // Arrange
        Country sut = CreateCountry();

        ContestId contestId = ContestId.FromValue(Guid.Parse("905d7fcc-87b7-46e6-a5e0-f5615400ccd6"));

        // Assert
        await Assert.That(sut.ParticipatingContestIds).IsEmpty();

        // Act
        sut.AddParticipatingContestId(contestId);

        // Assert
        ContestId? singleContestId = await Assert.That(sut.ParticipatingContestIds).HasSingleItem();

        await Assert.That(singleContestId).IsNotNull()
            .And.IsEqualTo(contestId)
            .And.IsNotSameReferenceAs(contestId);
    }

    [Test]
    public async Task AddParticipatingContestId_should_throw_given_null_contestId_arg()
    {
        // Arrange
        Country sut = CreateCountry();

        Action act = () => sut.AddParticipatingContestId(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'contestId')")
            .WithParameterName("contestId");
    }

    [Test]
    public async Task AddParticipatingContestId_should_throw_given_contestId_arg_matching_existing_ParticipatingContestId_item()
    {
        // Arrange
        Country sut = CreateCountry();

        ContestId existingContestId = ContestId.FromValue(Guid.Parse("905d7fcc-87b7-46e6-a5e0-f5615400ccd6"));

        sut.AddParticipatingContestId(existingContestId);

        Action act = () => sut.AddParticipatingContestId(existingContestId);

        // Assert
        await Assert.That(act).Throws<ArgumentException>()
            .WithMessage("Country ParticipatingContestIds collection already contains provided ContestId value.");
    }
}
