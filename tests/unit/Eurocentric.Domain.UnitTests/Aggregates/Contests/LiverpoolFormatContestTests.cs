using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Aggregates.Utils;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests;

public static class LiverpoolFormatContestTests
{
    private static Action<Participant> AssertCorrectGroup0Participant(CountryId participatingCountryId)
    {
        CountryId expectedCountryId = participatingCountryId;

        return ([UsedImplicitly] participant) =>
        {
            Assert.Equal(expectedCountryId, participant.ParticipatingCountryId);
            Assert.Equal(ParticipantGroup.Zero, participant.ParticipantGroup);
            Assert.Null(participant.ActName);
            Assert.Null(participant.SongTitle);
        };
    }

    private static Action<Participant> AssertCorrectGroup1Participant(CountryId participatingCountryId,
        string actName,
        string songTitle)
    {
        CountryId expectedCountryId = participatingCountryId;
        ActName expectedActName = ActName.FromValue(actName).Value;
        SongTitle expectedSongTitle = SongTitle.FromValue(songTitle).Value;

        return ([UsedImplicitly] participant) =>
        {
            Assert.Equal(expectedCountryId, participant.ParticipatingCountryId);
            Assert.Equal(ParticipantGroup.One, participant.ParticipantGroup);
            Assert.Equal(expectedActName, participant.ActName);
            Assert.Equal(expectedSongTitle, participant.SongTitle);
        };
    }

    private static Action<Participant> AssertCorrectGroup2Participant(CountryId participatingCountryId,
        string actName,
        string songTitle)
    {
        CountryId expectedCountryId = participatingCountryId;
        ActName expectedActName = ActName.FromValue(actName).Value;
        SongTitle expectedSongTitle = SongTitle.FromValue(songTitle).Value;

        return ([UsedImplicitly] participant) =>
        {
            Assert.Equal(expectedCountryId, participant.ParticipatingCountryId);
            Assert.Equal(ParticipantGroup.Two, participant.ParticipantGroup);
            Assert.Equal(expectedActName, participant.ActName);
            Assert.Equal(expectedSongTitle, participant.SongTitle);
        };
    }

    public sealed class FluentBuilder : UnitTest
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("37c2b6a4-a81b-4395-808a-4bc74ecc5f60"));

        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2025);

        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");

        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");

        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

        [Fact]
        public void Should_return_new_LiverpoolFormatContest_given_valid_args_scenario_1()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(2025))
                .WithCityName(CityName.FromValue("Basel"))
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ActName.FromValue("AT Act"), SongTitle.FromValue("AT Song"))
                .AddGroup1Participant(beId, ActName.FromValue("BE Act"), SongTitle.FromValue("BE Song"))
                .AddGroup1Participant(czId, ActName.FromValue("CZ Act"), SongTitle.FromValue("CZ Song"))
                .AddGroup2Participant(dkId, ActName.FromValue("DK Act"), SongTitle.FromValue("DK Song"))
                .AddGroup2Participant(eeId, ActName.FromValue("EE Act"), SongTitle.FromValue("EE Song"))
                .AddGroup2Participant(fiId, ActName.FromValue("FI Act"), SongTitle.FromValue("FI Song"))
                .Build(() => FixedContestId);

            (bool isError, Contest contest) = (errorsOrContest.IsError, errorsOrContest.Value);

            // Assert
            Assert.False(isError);

            LiverpoolFormatContest createdContest = Assert.IsType<LiverpoolFormatContest>(contest);

            Assert.Equal(FixedContestId, createdContest.Id);
            Assert.Equal(2025, createdContest.ContestYear.Value);
            Assert.Equal("Basel", createdContest.CityName.Value);
            Assert.Equal(ContestFormat.Liverpool, createdContest.ContestFormat);
            Assert.False(createdContest.Completed);
            Assert.Empty(createdContest.ChildBroadcasts);

            Assert.Collection(createdContest.Participants, AssertCorrectGroup0Participant(gbId),
                AssertCorrectGroup1Participant(atId, "AT Act", "AT Song"),
                AssertCorrectGroup1Participant(beId, "BE Act", "BE Song"),
                AssertCorrectGroup1Participant(czId, "CZ Act", "CZ Song"),
                AssertCorrectGroup2Participant(dkId, "DK Act", "DK Song"),
                AssertCorrectGroup2Participant(eeId, "EE Act", "EE Song"),
                AssertCorrectGroup2Participant(fiId, "FI Act", "FI Song"));
        }

        [Fact]
        public void Should_return_new_LiverpoolFormatContest_given_valid_args_scenario_2()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(2023))
                .WithCityName(CityName.FromValue("Liverpool"))
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ActName.FromValue("AT Act"), SongTitle.FromValue("AT Song"))
                .AddGroup2Participant(beId, ActName.FromValue("BE Act"), SongTitle.FromValue("BE Song"))
                .AddGroup1Participant(czId, ActName.FromValue("CZ Act"), SongTitle.FromValue("CZ Song"))
                .AddGroup2Participant(dkId, ActName.FromValue("DK Act"), SongTitle.FromValue("DK Song"))
                .AddGroup1Participant(eeId, ActName.FromValue("EE Act"), SongTitle.FromValue("EE Song"))
                .AddGroup2Participant(fiId, ActName.FromValue("FI Act"), SongTitle.FromValue("FI Song"))
                .Build(() => FixedContestId);

            (bool isError, Contest contest) = (errorsOrContest.IsError, errorsOrContest.Value);

            // Assert
            Assert.False(isError);

            LiverpoolFormatContest createdContest = Assert.IsType<LiverpoolFormatContest>(contest);

            Assert.Equal(FixedContestId, createdContest.Id);
            Assert.Equal(2023, createdContest.ContestYear.Value);
            Assert.Equal("Liverpool", createdContest.CityName.Value);
            Assert.Equal(ContestFormat.Liverpool, createdContest.ContestFormat);
            Assert.False(createdContest.Completed);
            Assert.Empty(createdContest.ChildBroadcasts);

            Assert.Collection(createdContest.Participants, AssertCorrectGroup0Participant(gbId),
                AssertCorrectGroup1Participant(atId, "AT Act", "AT Song"),
                AssertCorrectGroup1Participant(czId, "CZ Act", "CZ Song"),
                AssertCorrectGroup1Participant(eeId, "EE Act", "EE Song"),
                AssertCorrectGroup2Participant(beId, "BE Act", "BE Song"),
                AssertCorrectGroup2Participant(dkId, "DK Act", "DK Song"),
                AssertCorrectGroup2Participant(fiId, "FI Act", "FI Song"));
        }

        [Fact]
        public void Should_return_Errors_when_contest_year_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Contest year not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_city_name_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithContestYear(ArbitraryContestYear)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("City name not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_participants_not_added()
        {
            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format participant groups", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single group 0 participant, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_illegal_contest_year_value()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            const int contestYear = 0;

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(contestYear))
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal contest year value", firstError.Code);
            Assert.Equal("Contest year value must be an integer between 2016 and 2050.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestYear", Value: contestYear });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_city_name_value()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            const string cityName = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithCityName(CityName.FromValue(cityName))
                .WithContestYear(ArbitraryContestYear)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal("City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "cityName", Value: cityName });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_group_1_participant_act_name_value()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            const string actName = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup1Participant(atId, ActName.FromValue(actName), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: actName });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_group_2_participant_act_name_value()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            const string actName = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup2Participant(fiId, ActName.FromValue(actName), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: actName });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_group_1_participant_song_title_value()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            const string songTitle = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup1Participant(atId, ArbitraryActName, SongTitle.FromValue(songTitle))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: songTitle });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_group_2_participant_song_title_value()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            const string songTitle = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup2Participant(fiId, ArbitraryActName, SongTitle.FromValue(songTitle))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: songTitle });
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_group_0_and_group_1_participating_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup0Participant(gbId)
                .AddGroup1Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Each participant in a contest must have a participating country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_group_0_and_group_2_participating_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup0Participant(gbId)
                .AddGroup2Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Each participant in a contest must have a participating country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_group_1_and_group_2_participating_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId,
                CountryId huId) = new EightCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup1Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(huId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Each participant in a contest must have a participating country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_group_1_participating_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId,
                CountryId huId) = new EightCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup1Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(huId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Each participant in a contest must have a participating country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_group_2_participating_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId,
                CountryId huId) = new EightCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup2Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(huId)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal("Each participant in a contest must have a participating country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_zero_group_0_participants()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format participant groups", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single group 0 participant, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_more_than_one_group_0_participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId,
                CountryId huId) = new EightCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup0Participant(gbId)
                .AddGroup0Participant(huId)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(atId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format participant groups", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single group 0 participant, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_three_group_1_participants()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(atId)
                .AddGroup2Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format participant groups", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single group 0 participant, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_three_group_2_participants()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = LiverpoolFormatContest.Create()
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(atId)
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(dkId, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format participant groups", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single group 0 participant, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }
    }
}
