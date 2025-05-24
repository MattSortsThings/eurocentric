using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.UnitTests.Contests;

public sealed class LiverpoolFormatContestTests : UnitTestBase
{
    private static readonly ContestId FixedContestId =
        ContestId.FromValue(Guid.Parse("989c773a-0306-4136-813d-c10d8de2935b"));

    private static readonly BroadcastId FixedBroadcastId =
        BroadcastId.FromValue(Guid.Parse("10c39d7b-7a69-4589-97ec-6388abb6157e"));

    public sealed class FluentBuilder : UnitTestBase
    {
        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2016);
        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");
        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");
        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

        [Theory]
        [InlineData(2023, "Liverpool")]
        [InlineData(2025, "Basel")]
        public void Should_return_contest_with_provided_contest_year_and_city_name_and_Liverpool_format(int contestYear,
            string cityName)
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithYear(ContestYear.FromValue(contestYear))
                .WithCityName(CityName.FromValue(cityName))
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            LiverpoolFormatContest contest = Assert.IsType<LiverpoolFormatContest>(result);

            Assert.Equal(FixedContestId, contest.Id);
            Assert.Equal(contestYear, contest.Year.Value);
            Assert.Equal(cityName, contest.CityName.Value);
            Assert.Equal(ContestFormat.Liverpool, contest.Format);
        }

        [Fact]
        public void Should_return_contest_with_provided_participants_scenario_1_of_2()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ActName.FromValue("AT Act"), SongTitle.FromValue("AT Song"))
                .WithGroupOneParticipant(CountryIds.Be, ActName.FromValue("BE Act"), SongTitle.FromValue("BE Song"))
                .WithGroupOneParticipant(CountryIds.Cz, ActName.FromValue("CZ Act"), SongTitle.FromValue("CZ Song"))
                .WithGroupTwoParticipant(CountryIds.De, ActName.FromValue("DE Act"), SongTitle.FromValue("DE Song"))
                .WithGroupTwoParticipant(CountryIds.Ee, ActName.FromValue("ES Act"), SongTitle.FromValue("ES Song"))
                .WithGroupTwoParticipant(CountryIds.Fi, ActName.FromValue("FI Act"), SongTitle.FromValue("FI Song"))
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .Build(() => FixedContestId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            LiverpoolFormatContest contest = Assert.IsType<LiverpoolFormatContest>(result);

            Assert.Collection(contest.Participants, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Zero, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Xx);
                Assert.Null(participant.ActName);
                Assert.Null(participant.SongTitle);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.At);
                Assert.NotNull(participant.ActName);
                Assert.Equal("AT Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("AT Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Be);
                Assert.NotNull(participant.ActName);
                Assert.Equal("BE Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("BE Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Cz);
                Assert.NotNull(participant.ActName);
                Assert.Equal("CZ Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("CZ Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.De);
                Assert.NotNull(participant.ActName);
                Assert.Equal("DE Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("DE Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Ee);
                Assert.NotNull(participant.ActName);
                Assert.Equal("ES Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("ES Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Fi);
                Assert.NotNull(participant.ActName);
                Assert.Equal("FI Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("FI Song", participant.SongTitle.Value);
            });
        }

        [Fact]
        public void Should_return_contest_with_provided_participants_scenario_2_of_2()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ActName.FromValue("AT Act"), SongTitle.FromValue("AT Song"))
                .WithGroupTwoParticipant(CountryIds.Be, ActName.FromValue("BE Act"), SongTitle.FromValue("BE Song"))
                .WithGroupOneParticipant(CountryIds.Cz, ActName.FromValue("CZ Act"), SongTitle.FromValue("CZ Song"))
                .WithGroupTwoParticipant(CountryIds.De, ActName.FromValue("DE Act"), SongTitle.FromValue("DE Song"))
                .WithGroupOneParticipant(CountryIds.Ee, ActName.FromValue("ES Act"), SongTitle.FromValue("ES Song"))
                .WithGroupTwoParticipant(CountryIds.Fi, ActName.FromValue("FI Act"), SongTitle.FromValue("FI Song"))
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .Build(() => FixedContestId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            LiverpoolFormatContest contest = Assert.IsType<LiverpoolFormatContest>(result);

            Assert.Collection(contest.Participants, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Zero, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Xx);
                Assert.Null(participant.ActName);
                Assert.Null(participant.SongTitle);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.At);
                Assert.NotNull(participant.ActName);
                Assert.Equal("AT Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("AT Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Cz);
                Assert.NotNull(participant.ActName);
                Assert.Equal("CZ Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("CZ Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Ee);
                Assert.NotNull(participant.ActName);
                Assert.Equal("ES Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("ES Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Be);
                Assert.NotNull(participant.ActName);
                Assert.Equal("BE Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("BE Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.De);
                Assert.NotNull(participant.ActName);
                Assert.Equal("DE Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("DE Song", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.Group);
                Assert.Equal(participant.ParticipatingCountryId, CountryIds.Fi);
                Assert.NotNull(participant.ActName);
                Assert.Equal("FI Act", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("FI Song", participant.SongTitle.Value);
            });
        }

        [Fact]
        public void Should_return_contest_with_Initialized_status_and_no_broadcast_memos()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            LiverpoolFormatContest contest = Assert.IsType<LiverpoolFormatContest>(result);

            Assert.Equal(ContestStatus.Initialized, contest.Status);
            Assert.Empty(contest.BroadcastMemos);
        }

        [Fact]
        public void Should_return_errors_given_illegal_contest_year_value()
        {
            // Arrange
            const int illegalContestYear = 0;

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithYear(ContestYear.FromValue(illegalContestYear))
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal contest year value", firstError.Code);
            Assert.Equal("Contest year value must be an integer between 2016 and 2050.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestYear", Value: illegalContestYear });
        }

        [Fact]
        public void Should_return_errors_given_illegal_city_name_value()
        {
            // Arrange
            const string illegalCityName = "";

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithCityName(CityName.FromValue(illegalCityName))
                .WithYear(ArbitraryContestYear)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal("City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "cityName", Value: illegalCityName });
        }

        [Fact]
        public void Should_return_errors_given_group_1_participant_with_illegal_act_name_value()
        {
            // Arrange
            const string illegalActName = "";

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.At, ActName.FromValue(illegalActName), ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: illegalActName });
        }

        [Fact]
        public void Should_return_errors_given_group_1_participant_with_illegal_song_title_value()
        {
            // Arrange
            const string illegalSongTitle = "";

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, SongTitle.FromValue(illegalSongTitle))
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: illegalSongTitle });
        }

        [Fact]
        public void Should_return_errors_given_group_2_participant_with_illegal_act_name_value()
        {
            // Arrange
            const string illegalActName = "";

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(CountryIds.Fi, ActName.FromValue(illegalActName), ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: illegalActName });
        }

        [Fact]
        public void Should_return_errors_given_group_2_participant_with_illegal_song_title_value()
        {
            // Arrange
            const string illegalSongTitle = "";

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, SongTitle.FromValue(illegalSongTitle))
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: illegalSongTitle });
        }

        [Fact]
        public void Should_return_errors_given_group_0_and_group_1_participants_with_equal_participating_country_ID_values()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.Xx, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Every participant in a contest must have a different participating country ID.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_group_0_and_group_2_participants_with_equal_participating_country_ID_values()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupTwoParticipant(CountryIds.Xx, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Every participant in a contest must have a different participating country ID.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_group_1_and_group_2_participants_with_equal_participating_country_ID_values()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Every participant in a contest must have a different participating country ID.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_group_1_participants_with_equal_participating_country_ID_values()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Every participant in a contest must have a different participating country ID.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_group_2_participants_with_equal_participating_country_ID_values()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Every participant in a contest must have a different participating country ID.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_zero_group_0_participants()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have 1 Group 0 participant, at least 3 Group 1 participants, " +
                         "and at least 3 Group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_multiple_group_0_participants()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupZeroParticipant(CountryIds.It)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have 1 Group 0 participant, at least 3 Group 1 participants, " +
                         "and at least 3 Group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_two_group_1_participants()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have 1 Group 0 participant, at least 3 Group 1 participants, " +
                         "and at least 3 Group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_two_group_2_participants()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have 1 Group 0 participant, at least 3 Group 1 participants, " +
                         "and at least 3 Group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_throw_given_null_group_0_participant_countryId_arg()
        {
            // Arrange
            Func<ContestId> dummyIdProvider = () => FixedContestId;

            // Act
            Action act = () => LiverpoolFormatContest.Create()
                .WithGroupZeroParticipant(null!)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(dummyIdProvider);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_1_participant_countryId_arg()
        {
            // Arrange
            Func<ContestId> dummyIdProvider = () => FixedContestId;

            // Act
            Action act = () => LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(null!, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(dummyIdProvider);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_2_participant_countryId_arg()
        {
            // Arrange
            Func<ContestId> dummyIdProvider = () => FixedContestId;

            // Act
            Action act = () => LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(null!, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(dummyIdProvider);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_return_errors_given_null_IDProvider_arg()
        {
            // Act
            Action act = () => LiverpoolFormatContest.Create()
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }

    public sealed class AddMemoMethod : UnitTestBase
    {
        private static LiverpoolFormatContest InitializeContest()
        {
            ErrorOr<ContestYear> arbitraryContestYear = ContestYear.FromValue(2016);
            ErrorOr<CityName> arbitraryCityName = CityName.FromValue("CityName");
            ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
            ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

            Contest contest = LiverpoolFormatContest.Create()
                .WithYear(arbitraryContestYear)
                .WithCityName(arbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, arbitraryActName, arbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, arbitraryActName, arbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, arbitraryActName, arbitrarySongTitle)
                .Build(() => FixedContestId)
                .Value;

            return contest as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Theory]
        [InlineData(ContestStage.SemiFinal1)]
        [InlineData(ContestStage.SemiFinal2)]
        [InlineData(ContestStage.GrandFinal)]
        public void Should_add_memo_with_provided_broadcast_ID_and_contest_stage_values_and_Initialized_status(
            ContestStage contestStage)
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            BroadcastId broadcastId = BroadcastIds.GetOne();

            // Assert
            Assert.Empty(sut.BroadcastMemos);

            // Act
            sut.AddMemo(broadcastId, contestStage);

            // Assert
            BroadcastMemo memo = Assert.Single(sut.BroadcastMemos);

            Assert.Equal(broadcastId, memo.BroadcastId);
            Assert.Equal(contestStage, memo.ContestStage);
            Assert.Equal(BroadcastStatus.Initialized, memo.Status);
        }

        [Fact]
        public void Should_update_status_to_InProgress_when_first_memo_added()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            BroadcastId broadcastId = BroadcastIds.GetOne();

            // Assert
            Assert.Empty(sut.BroadcastMemos);
            Assert.Equal(ContestStatus.Initialized, sut.Status);

            // Act
            sut.AddMemo(broadcastId, ContestStage.SemiFinal1);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
        }

        [Fact]
        public void Should_not_update_status_when_second_memo_added()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of2, broadcastId2Of2) = BroadcastIds.GetTwo();

            sut.AddMemo(broadcastId1Of2, ContestStage.SemiFinal1);

            // Assert
            Assert.Single(sut.BroadcastMemos);
            Assert.Equal(ContestStatus.InProgress, sut.Status);

            // Act
            sut.AddMemo(broadcastId2Of2, ContestStage.SemiFinal2);

            // Assert
            Assert.Equal(2, sut.BroadcastMemos.Count);
            Assert.Equal(ContestStatus.InProgress, sut.Status);
        }

        [Fact]
        public void Should_not_update_status_when_third_memo_added()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of3, broadcastId2Of3, broadcastId3Of3) = BroadcastIds.GetThree();

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);

            // Assert
            Assert.Equal(2, sut.BroadcastMemos.Count);
            Assert.Equal(ContestStatus.InProgress, sut.Status);

            // Act
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            // Assert
            Assert.Equal(3, sut.BroadcastMemos.Count);
            Assert.Equal(ContestStatus.InProgress, sut.Status);
        }

        [Fact]
        public void Should_throw_when_has_memo_with_provided_broadcast_ID_value()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            BroadcastId broadcastId = BroadcastIds.GetOne();

            sut.AddMemo(broadcastId, ContestStage.SemiFinal1);

            // Assert
            BroadcastMemo initialMemo = Assert.Single(sut.BroadcastMemos);

            // Act
            Action act = () => sut.AddMemo(broadcastId, ContestStage.GrandFinal);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains an item with the provided BroadcastId value.", exception.Message);

            BroadcastMemo memo = Assert.Single(sut.BroadcastMemos);
            Assert.Equal(initialMemo, memo);
        }

        [Fact]
        public void Should_throw_when_has_memo_with_provided_contest_stage_value()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of2, broadcastId2Of2) = BroadcastIds.GetTwo();

            sut.AddMemo(broadcastId1Of2, ContestStage.SemiFinal1);

            // Assert
            BroadcastMemo initialMemo = Assert.Single(sut.BroadcastMemos);

            // Act
            Action act = () => sut.AddMemo(broadcastId2Of2, ContestStage.SemiFinal1);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains an item with the provided ContestStage value.", exception.Message);

            BroadcastMemo memo = Assert.Single(sut.BroadcastMemos);
            Assert.Equal(initialMemo, memo);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            // Act
            Action act = () => sut.AddMemo(null!, ContestStage.SemiFinal1);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);
        }
    }

    public sealed class ReplaceMemoMethod : UnitTestBase
    {
        private static LiverpoolFormatContest InitializeContest()
        {
            ErrorOr<ContestYear> arbitraryContestYear = ContestYear.FromValue(2016);
            ErrorOr<CityName> arbitraryCityName = CityName.FromValue("CityName");
            ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
            ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

            Contest contest = LiverpoolFormatContest.Create()
                .WithYear(arbitraryContestYear)
                .WithCityName(arbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, arbitraryActName, arbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, arbitraryActName, arbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, arbitraryActName, arbitrarySongTitle)
                .Build(() => FixedContestId)
                .Value;

            return contest as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_replace_memo_with_provided_broadcast_ID_and_status_values()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of2, broadcastId2Of2) = BroadcastIds.GetTwo();

            sut.AddMemo(broadcastId1Of2, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of2, ContestStage.SemiFinal2);

            // Assert
            Assert.Equivalent((BroadcastMemo[])
            [
                new BroadcastMemo(broadcastId1Of2, ContestStage.SemiFinal1, BroadcastStatus.Initialized),
                new BroadcastMemo(broadcastId2Of2, ContestStage.SemiFinal2, BroadcastStatus.Initialized)
            ], sut.BroadcastMemos);

            // Act
            sut.ReplaceMemo(broadcastId2Of2, BroadcastStatus.InProgress);

            // Assert
            Assert.Equivalent((BroadcastMemo[])
            [
                new BroadcastMemo(broadcastId1Of2, ContestStage.SemiFinal1, BroadcastStatus.Initialized),
                new BroadcastMemo(broadcastId2Of2, ContestStage.SemiFinal2, BroadcastStatus.InProgress)
            ], sut.BroadcastMemos);
        }

        [Fact]
        public void Should_update_status_to_Completed_when_has_three_memos_and_replacement_leaves_all_Completed()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of3, broadcastId2Of3, broadcastId3Of3) = BroadcastIds.GetThree();

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            sut.ReplaceMemo(broadcastId1Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId2Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId3Of3, BroadcastStatus.InProgress);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(3, sut.BroadcastMemos.Count);
            Assert.False(sut.BroadcastMemos.All(memo => memo.Status == BroadcastStatus.Completed));

            // Act
            sut.ReplaceMemo(broadcastId3Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.Completed, sut.Status);
            Assert.Equal(3, sut.BroadcastMemos.Count);
            Assert.True(sut.BroadcastMemos.All(memo => memo.Status == BroadcastStatus.Completed));
        }

        [Fact]
        public void Should_not_update_status_when_has_three_memos_and_replacement_does_not_leave_all_Completed()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of3, broadcastId2Of3, broadcastId3Of3) = BroadcastIds.GetThree();

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            sut.ReplaceMemo(broadcastId1Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId2Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(3, sut.BroadcastMemos.Count);
            Assert.False(sut.BroadcastMemos.All(memo => memo.Status == BroadcastStatus.Completed));

            // Act
            sut.ReplaceMemo(broadcastId3Of3, BroadcastStatus.InProgress);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(3, sut.BroadcastMemos.Count);
            Assert.False(sut.BroadcastMemos.All(memo => memo.Status == BroadcastStatus.Completed));
        }

        [Fact]
        public void Should_not_update_status_when_has_two_memos_and_one_is_replaced()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of3, broadcastId2Of3) = BroadcastIds.GetTwo();

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(2, sut.BroadcastMemos.Count);

            // Act
            sut.ReplaceMemo(broadcastId2Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(2, sut.BroadcastMemos.Count);
        }

        [Fact]
        public void Should_not_update_status_when_has_one_memo_and_it_is_replaced()
        {
            LiverpoolFormatContest sut = InitializeContest();

            BroadcastId broadcastId = BroadcastIds.GetOne();

            sut.AddMemo(broadcastId, ContestStage.SemiFinal1);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Single(sut.BroadcastMemos);

            // Act
            sut.ReplaceMemo(broadcastId, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Single(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_when_has_no_memo_with_provided_broadcast_ID_value()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            BroadcastId broadcastId = BroadcastIds.GetOne();

            // Assert
            Assert.Empty(sut.BroadcastMemos);

            // Act
            Action act = () => sut.ReplaceMemo(broadcastId, BroadcastStatus.InProgress);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains no item with the provided BroadcastId value.", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            // Act
            Action act = () => sut.AddMemo(null!, ContestStage.SemiFinal1);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);
        }
    }

    public sealed class RemoveMemoMethod : UnitTestBase
    {
        private static LiverpoolFormatContest InitializeContest()
        {
            ErrorOr<ContestYear> arbitraryContestYear = ContestYear.FromValue(2016);
            ErrorOr<CityName> arbitraryCityName = CityName.FromValue("CityName");
            ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
            ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

            Contest contest = LiverpoolFormatContest.Create()
                .WithYear(arbitraryContestYear)
                .WithCityName(arbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, arbitraryActName, arbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, arbitraryActName, arbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Ee, arbitraryActName, arbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, arbitraryActName, arbitrarySongTitle)
                .Build(() => FixedContestId)
                .Value;

            return contest as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_remove_memo_with_provided_broadcast_ID()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of2, broadcastId2Of2) = BroadcastIds.GetTwo();

            sut.AddMemo(broadcastId1Of2, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of2, ContestStage.SemiFinal2);

            // Assert
            Assert.Equivalent((BroadcastMemo[])
            [
                new BroadcastMemo(broadcastId1Of2, ContestStage.SemiFinal1, BroadcastStatus.Initialized),
                new BroadcastMemo(broadcastId2Of2, ContestStage.SemiFinal2, BroadcastStatus.Initialized)
            ], sut.BroadcastMemos);

            // Act
            sut.RemoveMemo(broadcastId2Of2);

            // Assert
            BroadcastMemo remainingMemo = Assert.Single(sut.BroadcastMemos);
            Assert.Equal(new BroadcastMemo(broadcastId1Of2, ContestStage.SemiFinal1, BroadcastStatus.Initialized),
                remainingMemo);
        }

        [Fact]
        public void Should_update_status_to_InProgress_when_has_three_memos_all_Completed_and_one_is_removed()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of3, broadcastId2Of3, broadcastId3Of3) = BroadcastIds.GetThree();

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            sut.ReplaceMemo(broadcastId1Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId2Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId3Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.Completed, sut.Status);
            Assert.Equal(3, sut.BroadcastMemos.Count);
            Assert.True(sut.BroadcastMemos.All(memo => memo.Status == BroadcastStatus.Completed));

            // Act
            sut.RemoveMemo(broadcastId3Of3);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(2, sut.BroadcastMemos.Count);
        }

        [Fact]
        public void Should_not_update_status_when_has_three_memos_not_all_Completed_and_one_is_removed()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of3, broadcastId2Of3, broadcastId3Of3) = BroadcastIds.GetThree();

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            sut.ReplaceMemo(broadcastId1Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId2Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(3, sut.BroadcastMemos.Count);
            Assert.False(sut.BroadcastMemos.All(memo => memo.Status == BroadcastStatus.Completed));

            // Act
            sut.RemoveMemo(broadcastId3Of3);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(2, sut.BroadcastMemos.Count);
        }

        [Fact]
        public void Should_not_update_status_when_has_two_memos_and_one_is_removed()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            var (broadcastId1Of2, broadcastId2Of2) = BroadcastIds.GetTwo();

            sut.AddMemo(broadcastId1Of2, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of2, ContestStage.SemiFinal2);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Equal(2, sut.BroadcastMemos.Count);

            // Act
            sut.RemoveMemo(broadcastId1Of2);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Single(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_update_status_to_Initialized_when_has_one_memo_and_it_is_removed()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            BroadcastId broadcastId = BroadcastIds.GetOne();

            sut.AddMemo(broadcastId, ContestStage.SemiFinal1);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.Status);
            Assert.Single(sut.BroadcastMemos);

            // Act
            sut.RemoveMemo(broadcastId);

            // Assert
            Assert.Equal(ContestStatus.Initialized, sut.Status);
            Assert.Empty(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_when_has_no_memo_with_provided_broadcast_ID_value()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            BroadcastId broadcastId = BroadcastIds.GetOne();

            // Assert
            Assert.Empty(sut.BroadcastMemos);

            // Act
            Action act = () => sut.RemoveMemo(broadcastId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains no item with the provided BroadcastId value.", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest();

            // Act
            Action act = () => sut.AddMemo(null!, ContestStage.SemiFinal1);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);
        }
    }

    public sealed class CreateSemiFinal1BroadcastMethod : UnitTestBase
    {
        private static LiverpoolFormatContest InitializeContest(CountryId[]? group1Participants = null,
            CountryId[]? group2Participants = null,
            CountryId? group0Participant = null)
        {
            ErrorOr<ContestYear> arbitraryContestYear = ContestYear.FromValue(2016);
            ErrorOr<CityName> arbitraryCityName = CityName.FromValue("CityName");
            ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
            ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

            ContestBuilder builder = LiverpoolFormatContest.Create()
                .WithYear(arbitraryContestYear)
                .WithCityName(arbitraryCityName);

            if (group0Participant is not null)
            {
                builder.WithGroupZeroParticipant(group0Participant);
            }

            foreach (CountryId countryId in group1Participants ?? [])
            {
                builder.WithGroupOneParticipant(countryId, arbitraryActName, arbitrarySongTitle);
            }

            foreach (CountryId countryId in group2Participants ?? [])
            {
                builder.WithGroupTwoParticipant(countryId, arbitraryActName, arbitrarySongTitle);
            }

            Contest contest = builder.Build(() => FixedContestId).Value;

            return contest as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_return_broadcast_with_own_contest_ID_and_SemiFinal1_contest_stage_and_Initialized_status()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Be, CountryIds.Cz];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            (bool isError, Broadcast result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equal(FixedBroadcastId, result.Id);
            Assert.Equal(sut.Id, result.ContestId);
            Assert.Equal(ContestStage.SemiFinal1, result.ContestStage);
        }

        [Fact]
        public void Should_return_broadcast_with_competitors_running_order_matching_provided_competing_country_IDs_order()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.Cz, CountryIds.At, CountryIds.Be];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equal(competingCountryIds, result.Competitors.Select(competitor => competitor.CompetingCountryId));
            Assert.Equal([1, 2, 3], result.Competitors.Select(competitor => competitor.RunningOrderPosition));
        }

        [Fact]
        public void Should_return_broadcast_with_all_competitors_having_finishing_position_equal_to_running_order()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Be, CountryIds.Cz];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.All(result.Competitors, competitor =>
                Assert.True(competitor.RunningOrderPosition == competitor.FinishingPosition));
        }

        [Fact]
        public void Should_return_broadcast_with_all_competitors_having_no_jury_awards_and_no_televote_awards()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Be, CountryIds.Cz];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.All(result.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            });
        }

        [Fact]
        public void Should_return_broadcast_with_no_juries()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Be];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Empty(result.Juries);
        }

        [Fact]
        public void Should_return_broadcast_with_a_televote_for_every_group_0_and_group_1_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Be];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equivalent((CountryId[]) [CountryIds.Xx, CountryIds.At, CountryIds.Be, CountryIds.Cz],
                result.Televotes.Select(televote => televote.VotingCountryId));
            Assert.All(result.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_errors_when_SemiFinal1_broadcast_memo_already_exists()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Be];

            sut.AddMemo(BroadcastIds.GetOne(), ContestStage.SemiFinal1);

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Broadcast contest stage conflict", firstError.Code);
            Assert.Equal("Contest already has a child broadcast with the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal1" });
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_two_competitors()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast size", firstError.Code);
            Assert.Equal("Broadcast must have at least 2 competitors.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_competitors_with_duplicate_competing_country_IDs()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Be, CountryIds.Cz, CountryIds.At];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must have a different competing country ID.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_with_no_matching_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.It];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal1" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.It.Value));
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_matching_group_0_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Xx];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal1" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.Xx.Value));
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_matching_group_2_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.De];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal1Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal1" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.De.Value));
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            // Act
            Action act = () => sut.CreateSemiFinal1Broadcast(null!, () => FixedBroadcastId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] dummyCountryIds = [];

            // Act
            Action act = () => sut.CreateSemiFinal1Broadcast(dummyCountryIds, null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }

    public sealed class CreateSemiFinal2BroadcastMethod : UnitTestBase
    {
        private static LiverpoolFormatContest InitializeContest(CountryId[]? group1Participants = null,
            CountryId[]? group2Participants = null,
            CountryId? group0Participant = null)
        {
            ErrorOr<ContestYear> arbitraryContestYear = ContestYear.FromValue(2016);
            ErrorOr<CityName> arbitraryCityName = CityName.FromValue("CityName");
            ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
            ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

            ContestBuilder builder = LiverpoolFormatContest.Create()
                .WithYear(arbitraryContestYear)
                .WithCityName(arbitraryCityName);

            if (group0Participant is not null)
            {
                builder.WithGroupZeroParticipant(group0Participant);
            }

            foreach (CountryId countryId in group1Participants ?? [])
            {
                builder.WithGroupOneParticipant(countryId, arbitraryActName, arbitrarySongTitle);
            }

            foreach (CountryId countryId in group2Participants ?? [])
            {
                builder.WithGroupTwoParticipant(countryId, arbitraryActName, arbitrarySongTitle);
            }

            Contest contest = builder.Build(() => FixedContestId).Value;

            return contest as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_return_broadcast_with_own_contest_ID_and_SemiFinal2_contest_stage_and_Initialized_status()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee, CountryIds.Fi];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            (bool isError, Broadcast result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equal(FixedBroadcastId, result.Id);
            Assert.Equal(sut.Id, result.ContestId);
            Assert.Equal(ContestStage.SemiFinal2, result.ContestStage);
        }

        [Fact]
        public void Should_return_broadcast_with_competitors_running_order_matching_provided_competing_country_IDs_order()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.Fi, CountryIds.Ee, CountryIds.De];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equal(competingCountryIds, result.Competitors.Select(competitor => competitor.CompetingCountryId));
            Assert.Equal([1, 2, 3], result.Competitors.Select(competitor => competitor.RunningOrderPosition));
        }

        [Fact]
        public void Should_return_broadcast_with_all_competitors_having_finishing_position_equal_to_running_order()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee, CountryIds.Fi];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.All(result.Competitors, competitor =>
                Assert.True(competitor.RunningOrderPosition == competitor.FinishingPosition));
        }

        [Fact]
        public void Should_return_broadcast_with_all_competitors_having_no_jury_awards_and_no_televote_awards()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee, CountryIds.Fi];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.All(result.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            });
        }

        [Fact]
        public void Should_return_broadcast_with_no_juries()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Empty(result.Juries);
        }

        [Fact]
        public void Should_return_broadcast_with_a_televote_for_every_group_0_and_group_2_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equivalent((CountryId[]) [CountryIds.Xx, CountryIds.De, CountryIds.Ee, CountryIds.Fi],
                result.Televotes.Select(televote => televote.VotingCountryId));
            Assert.All(result.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_errors_when_SemiFinal2_broadcast_memo_already_exists()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee];

            sut.AddMemo(BroadcastIds.GetOne(), ContestStage.SemiFinal2);

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Broadcast contest stage conflict", firstError.Code);
            Assert.Equal("Contest already has a child broadcast with the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal2" });
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_two_competitors()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast size", firstError.Code);
            Assert.Equal("Broadcast must have at least 2 competitors.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_competitors_with_duplicate_competing_country_IDs()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee, CountryIds.Fi, CountryIds.De];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must have a different competing country ID.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_with_no_matching_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.It];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal2" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.It.Value));
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_matching_group_0_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Xx];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal2" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.Xx.Value));
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_matching_group_1_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.At];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateSemiFinal2Broadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal2" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.At.Value));
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            // Act
            Action act = () => sut.CreateSemiFinal2Broadcast(null!, () => FixedBroadcastId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] dummyCountryIds = [];

            // Act
            Action act = () => sut.CreateSemiFinal2Broadcast(dummyCountryIds, null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }

    public sealed class CreateGrandFinalBroadcastMethod : UnitTestBase
    {
        private static LiverpoolFormatContest InitializeContest(CountryId[]? group1Participants = null,
            CountryId[]? group2Participants = null,
            CountryId? group0Participant = null)
        {
            ErrorOr<ContestYear> arbitraryContestYear = ContestYear.FromValue(2016);
            ErrorOr<CityName> arbitraryCityName = CityName.FromValue("CityName");
            ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
            ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

            ContestBuilder builder = LiverpoolFormatContest.Create()
                .WithYear(arbitraryContestYear)
                .WithCityName(arbitraryCityName);

            if (group0Participant is not null)
            {
                builder.WithGroupZeroParticipant(group0Participant);
            }

            foreach (CountryId countryId in group1Participants ?? [])
            {
                builder.WithGroupOneParticipant(countryId, arbitraryActName, arbitrarySongTitle);
            }

            foreach (CountryId countryId in group2Participants ?? [])
            {
                builder.WithGroupTwoParticipant(countryId, arbitraryActName, arbitrarySongTitle);
            }

            Contest contest = builder.Build(() => FixedContestId).Value;

            return contest as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_return_broadcast_with_own_contest_ID_and_GrandFinal_contest_stage_and_Initialized_status()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds =
            [
                CountryIds.At, CountryIds.Be, CountryIds.Cz, CountryIds.De, CountryIds.Ee, CountryIds.Fi
            ];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            (bool isError, Broadcast result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equal(FixedBroadcastId, result.Id);
            Assert.Equal(sut.Id, result.ContestId);
            Assert.Equal(ContestStage.GrandFinal, result.ContestStage);
        }

        [Fact]
        public void Should_return_broadcast_with_competitors_running_order_matching_provided_competing_country_IDs_order()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds =
            [
                CountryIds.Fi, CountryIds.At, CountryIds.Ee, CountryIds.Cz, CountryIds.Be, CountryIds.De
            ];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equal(competingCountryIds, result.Competitors.Select(competitor => competitor.CompetingCountryId));
            Assert.Equal([1, 2, 3, 4, 5, 6], result.Competitors.Select(competitor => competitor.RunningOrderPosition));
        }

        [Fact]
        public void Should_return_broadcast_with_all_competitors_having_finishing_position_equal_to_running_order()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds =
            [
                CountryIds.Fi, CountryIds.At, CountryIds.Ee, CountryIds.Cz, CountryIds.Be, CountryIds.De
            ];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.All(result.Competitors,
                competitor => Assert.True(competitor.RunningOrderPosition == competitor.FinishingPosition));
        }

        [Fact]
        public void Should_return_broadcast_with_all_competitors_having_no_jury_awards_and_no_televote_awards()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds =
            [
                CountryIds.Fi, CountryIds.At, CountryIds.Ee, CountryIds.Cz, CountryIds.Be, CountryIds.De
            ];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.All(result.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            });
        }

        [Fact]
        public void Should_return_broadcast_with_a_jury_for_every_group_1_and_group_2_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equivalent((CountryId[])
                [
                    CountryIds.At, CountryIds.Be, CountryIds.Cz, CountryIds.De, CountryIds.Ee, CountryIds.Fi
                ],
                result.Juries.Select(televote => televote.VotingCountryId));
            Assert.All(result.Juries, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_broadcast_with_a_televote_for_every_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.De, CountryIds.Ee];

            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);

            Assert.Equivalent((CountryId[])
                [
                    CountryIds.Xx, CountryIds.At, CountryIds.Be, CountryIds.Cz, CountryIds.De, CountryIds.Ee, CountryIds.Fi
                ],
                result.Televotes.Select(televote => televote.VotingCountryId));
            Assert.All(result.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_errors_when_GrandFinal_broadcast_memo_already_exists()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.De];

            sut.AddMemo(BroadcastIds.GetOne(), ContestStage.GrandFinal);

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Broadcast contest stage conflict", firstError.Code);
            Assert.Equal("Contest already has a child broadcast with the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "GrandFinal" });
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_two_competitors()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast size", firstError.Code);
            Assert.Equal("Broadcast must have at least 2 competitors.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_competitors_with_duplicate_competing_country_IDs()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.De, CountryIds.Be, CountryIds.De];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must have a different competing country ID.", firstError.Description);
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_with_no_matching_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.It];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "GrandFinal" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.It.Value));
        }

        [Fact]
        public void Should_return_errors_given_competing_country_ID_matching_group_0_participant()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] competingCountryIds = [CountryIds.At, CountryIds.Xx];

            // Act
            ErrorOr<Broadcast> errorsOrResult = sut.CreateGrandFinalBroadcast(competingCountryIds, () => FixedBroadcastId);

            var (isError, result, firstError) = (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Illegal competing country IDs", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must share a country ID with a contest participant " +
                         "eligible to compete in the requested contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "GrandFinal" });
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "illegalCompetingCountryIds", Value: Guid[] guidArray } &&
                       guidArray.Contains(CountryIds.Xx.Value));
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            // Act
            Action act = () => sut.CreateGrandFinalBroadcast(null!, () => FixedBroadcastId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = InitializeContest(group0Participant: CountryIds.Xx,
                group1Participants: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
                group2Participants: [CountryIds.De, CountryIds.Ee, CountryIds.Fi]);

            CountryId[] dummyCountryIds = [];

            // Act
            Action act = () => sut.CreateGrandFinalBroadcast(dummyCountryIds, null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }
}
