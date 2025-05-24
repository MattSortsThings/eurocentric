using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Contests;

public sealed class ParticipantTests : UnitTestBase
{
    private static readonly CountryId FixedCountryId = CountryId.FromValue(Guid.Parse("a66d83d5-3aa8-4415-81c3-420c0eb0c87a"));
    private static readonly ActName FixedActName = ActName.FromValue("ActName").Value;
    private static readonly SongTitle FixedSongTitle = SongTitle.FromValue("SongTitle").Value;

    public sealed class CreateInGroupZeroStaticMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_provided_country_ID_and_group_Zero_and_null_act_name_and_null_song_title()
        {
            // Act
            Participant result = Participant.CreateInGroupZero(FixedCountryId);

            // Assert
            Assert.Equal(FixedCountryId, result.ParticipatingCountryId);
            Assert.Equal(ParticipantGroup.Zero, result.Group);
            Assert.Null(result.ActName);
            Assert.Null(result.SongTitle);
        }

        [Fact]
        public void Should_throw_given_null_countryId_arg()
        {
            // Act
            Action act = () => Participant.CreateInGroupZero(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }
    }

    public sealed class CreateInGroupOneStaticMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_provided_country_ID_and_group_One_and_provided_act_name_and_provided_song_title()
        {
            // Act
            ErrorOr<Participant> errorsOrResult = Participant.CreateInGroupOne(FixedCountryId, FixedActName, FixedSongTitle);

            (bool isError, Participant participant) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(participant);
            Assert.Equal(FixedCountryId, participant.ParticipatingCountryId);
            Assert.Equal(ParticipantGroup.One, participant.Group);
            Assert.Equal(FixedActName, participant.ActName);
            Assert.Equal(FixedSongTitle, participant.SongTitle);
        }

        [Fact]
        public void Should_return_errors_when_actName_arg_is_errors()
        {
            // Arrange
            ErrorOr<ActName> illegalActName = ActName.FromValue(string.Empty);

            // Act
            ErrorOr<Participant> errorsOrResult = Participant.CreateInGroupOne(FixedCountryId, illegalActName, FixedSongTitle);

            (bool isError, Participant participant, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(participant);

            Assert.Equal("Illegal act name value", firstError.Code);
        }

        [Fact]
        public void Should_return_errors_when_songTitle_arg_is_errors()
        {
            // Arrange
            ErrorOr<SongTitle> illegalSongTitle = SongTitle.FromValue(string.Empty);

            // Act
            ErrorOr<Participant> errorsOrResult = Participant.CreateInGroupOne(FixedCountryId, FixedActName, illegalSongTitle);

            (bool isError, Participant participant, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(participant);

            Assert.Equal("Illegal song title value", firstError.Code);
        }

        [Fact]
        public void Should_throw_given_null_countryId_arg()
        {
            // Act
            Action act = () => Participant.CreateInGroupOne(null!, FixedActName, FixedSongTitle);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }
    }

    public sealed class CreateInGroupTwoStaticMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_provided_country_ID_and_group_Two_and_provided_act_name_and_provided_song_title()
        {
            // Act
            ErrorOr<Participant> errorsOrResult = Participant.CreateInGroupTwo(FixedCountryId, FixedActName, FixedSongTitle);

            (bool isError, Participant participant) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(participant);
            Assert.Equal(FixedCountryId, participant.ParticipatingCountryId);
            Assert.Equal(ParticipantGroup.Two, participant.Group);
            Assert.Equal(FixedActName, participant.ActName);
            Assert.Equal(FixedSongTitle, participant.SongTitle);
        }

        [Fact]
        public void Should_return_errors_when_actName_arg_is_errors()
        {
            // Arrange
            ErrorOr<ActName> illegalActName = ActName.FromValue(string.Empty);

            // Act
            ErrorOr<Participant> errorsOrResult = Participant.CreateInGroupTwo(FixedCountryId, illegalActName, FixedSongTitle);

            (bool isError, Participant participant, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(participant);

            Assert.Equal("Illegal act name value", firstError.Code);
        }

        [Fact]
        public void Should_return_errors_when_songTitle_arg_is_errors()
        {
            // Arrange
            ErrorOr<SongTitle> illegalSongTitle = SongTitle.FromValue(string.Empty);

            // Act
            ErrorOr<Participant> errorsOrResult = Participant.CreateInGroupTwo(FixedCountryId, FixedActName, illegalSongTitle);

            (bool isError, Participant participant, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(participant);

            Assert.Equal("Illegal song title value", firstError.Code);
        }

        [Fact]
        public void Should_throw_given_null_countryId_arg()
        {
            // Act
            Action act = () => Participant.CreateInGroupTwo(null!, FixedActName, FixedSongTitle);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }
    }

    public sealed class CreateCompetitorMethod : UnitTestBase
    {
        private static Participant CreateGroup1ParticipantWithDefaultValues() =>
            Participant.CreateInGroupOne(FixedCountryId, FixedActName, FixedSongTitle).Value;

        [Fact]
        public void Should_return_competitor_with_own_participating_country_ID_and_provided_running_order_position()
        {
            // Arrange
            Participant sut = CreateGroup1ParticipantWithDefaultValues();

            const int runningOrderPosition = 7;

            // Act
            Competitor result = sut.CreateCompetitor(runningOrderPosition);

            // Assert
            Assert.Equal(sut.ParticipatingCountryId, result.CompetingCountryId);
            Assert.Equal(runningOrderPosition, result.RunningOrderPosition);
        }

        [Fact]
        public void Should_return_competitor_with_finishing_position_equal_to_provided_running_order_position()
        {
            // Arrange
            Participant sut = CreateGroup1ParticipantWithDefaultValues();

            const int runningOrderPosition = 7;

            // Act
            Competitor result = sut.CreateCompetitor(runningOrderPosition);

            // Assert
            Assert.Equal(runningOrderPosition, result.FinishingPosition);
        }

        [Fact]
        public void Should_return_competitor_with_empty_jury_awards_and_empty_televote_awards()
        {
            // Arrange
            Participant sut = CreateGroup1ParticipantWithDefaultValues();

            const int runningOrderPosition = 7;

            // Act
            Competitor result = sut.CreateCompetitor(runningOrderPosition);

            // Assert
            Assert.Empty(result.JuryAwards);
            Assert.Empty(result.TelevoteAwards);
        }
    }

    public sealed class CreateJuryMethod : UnitTestBase
    {
        private static Participant CreateGroup1ParticipantWithDefaultValues() =>
            Participant.CreateInGroupOne(FixedCountryId, FixedActName, FixedSongTitle).Value;

        [Fact]
        public void Should_return_jury_with_own_participating_country_ID_and_false_points_awarded_value()
        {
            // Arrange
            Participant sut = CreateGroup1ParticipantWithDefaultValues();

            // Act
            Jury result = sut.CreateJury();

            // Assert
            Assert.Equal(sut.ParticipatingCountryId, result.VotingCountryId);
            Assert.False(result.PointsAwarded);
        }
    }

    public sealed class CreateTelevoteMethod : UnitTestBase
    {
        private static Participant CreateGroup1ParticipantWithDefaultValues() =>
            Participant.CreateInGroupOne(FixedCountryId, FixedActName, FixedSongTitle).Value;

        [Fact]
        public void Should_return_televote_with_own_participating_country_ID_and_false_points_awarded_value()
        {
            // Arrange
            Participant sut = CreateGroup1ParticipantWithDefaultValues();

            // Act
            Televote result = sut.CreateTelevote();

            // Assert
            Assert.Equal(sut.ParticipatingCountryId, result.VotingCountryId);
            Assert.False(result.PointsAwarded);
        }
    }
}
