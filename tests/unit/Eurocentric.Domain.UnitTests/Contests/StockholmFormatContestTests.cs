using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.UnitTests.Contests;

public sealed class StockholmFormatContestTests : UnitTestBase
{
    public sealed class FluentBuilder : UnitTestBase
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

        private static readonly ContestYear ArbitraryContestYear = ContestYear.FromValue(2025).Value;
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
        private static readonly ActName ArbitraryActName = ActName.FromValue("ActName").Value;
        private static readonly SongTitle ArbitrarySongTitle = SongTitle.FromValue("SongTitle").Value;

        [Theory]
        [InlineData(2016, "Stockholm")]
        [InlineData(2022, "Turin")]
        public void Should_create_contest_given_legal_args_scenario(int contestYear, string cityName)
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear.FromValue(contestYear))
                .WithCityName(CityName.FromValue(cityName))
                .AddGroup1Participant(TestCountryIds.At, ActName.FromValue("Act AT"), SongTitle.FromValue("Song AT"))
                .AddGroup1Participant(TestCountryIds.Be, ActName.FromValue("Act BE"), SongTitle.FromValue("Song BE"))
                .AddGroup1Participant(TestCountryIds.Cz, ActName.FromValue("Act CZ"), SongTitle.FromValue("Song CZ"))
                .AddGroup2Participant(TestCountryIds.Dk, ActName.FromValue("Act DK"), SongTitle.FromValue("Song DK"))
                .AddGroup2Participant(TestCountryIds.Ee, ActName.FromValue("Act EE"), SongTitle.FromValue("Song EE"))
                .AddGroup2Participant(TestCountryIds.Fi, ActName.FromValue("Act FI"), SongTitle.FromValue("Song FI"))
                .Build(new FixedContestIdGenerator(FixedContestId));

            (bool isError, Contest contest) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            StockholmFormatContest createdContest = Assert.IsType<StockholmFormatContest>(contest);

            Assert.Equal(FixedContestId, createdContest.Id);
            Assert.Equal(contestYear, createdContest.ContestYear.Value);
            Assert.Equal(cityName, createdContest.CityName.Value);
            Assert.Equal(ContestFormat.Stockholm, createdContest.ContestFormat);
            Assert.Equal(ContestStatus.Initialized, createdContest.ContestStatus);
            Assert.Empty(createdContest.ChildBroadcasts);

            Assert.Collection(createdContest.Participants, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.At, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act AT", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song AT", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Be, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act BE", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song BE", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Cz, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act CZ", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song CZ", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Dk, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act DK", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song DK", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Ee, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act EE", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song EE", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Fi, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act FI", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song FI", participant.SongTitle.Value);
            });
        }

        [Fact]
        public void Should_return_errors_given_illegal_contest_year_value()
        {
            // Arrange
            const int illegalContestYearValue = 999999;

            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear.FromValue(illegalContestYearValue))
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal contest year value", firstError.Code);
            Assert.Equal("Contest year value must be an integer between 2016 and 2050.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestYear", Value: illegalContestYearValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_city_name_value()
        {
            // Arrange
            const string illegalCityNameValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .WithCityName(CityName.FromValue(illegalCityNameValue))
                .WithContestYear(ArbitraryContestYear)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal("City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "cityName", Value: illegalCityNameValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_1_participant_act_name_value()
        {
            // Arrange
            const string illegalActNameValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup1Participant(TestCountryIds.At, ActName.FromValue(illegalActNameValue), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: illegalActNameValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_2_participant_act_name_value()
        {
            // Arrange
            const string illegalActNameValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup2Participant(TestCountryIds.Fi, ActName.FromValue(illegalActNameValue), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: illegalActNameValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_1_participant_song_title_value()
        {
            // Arrange
            const string illegalSongTitleValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, SongTitle.FromValue(illegalSongTitleValue))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: illegalSongTitleValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_2_participant_song_title_value()
        {
            // Arrange
            const string illegalSongTitleValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, SongTitle.FromValue(illegalSongTitleValue))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: illegalSongTitleValue });
        }

        [Fact]
        public void Should_return_errors_given_non_zero_group_0_participants()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Stockholm format group sizes", firstError.Code);
            Assert.Equal("A Stockholm format contest must have no participants in group 0, " +
                         "at least 3 in group 1, and at least 3 in group 2.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_3_group_1_participants()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Stockholm format group sizes", firstError.Code);
            Assert.Equal("A Stockholm format contest must have no participants in group 0, " +
                         "at least 3 in group 1, and at least 3 in group 2.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_3_group_2_participants()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Stockholm format group sizes", firstError.Code);
            Assert.Equal("A Stockholm format contest must have no participants in group 0, " +
                         "at least 3 in group 1, and at least 3 in group 2.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_1_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup1Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_2_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup2Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_1_and_2_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateStockholmFormat()
                .AddGroup1Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_throw_given_null_group_0_participant_countryId_arg()
        {
            // Act
            Action act = () => Contest.CreateStockholmFormat()
                .AddGroup0Participant(null!)
                .Build(new DummyContestIdGenerator());

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_1_participant_countryId_arg()
        {
            // Act
            Action act = () => Contest.CreateStockholmFormat()
                .AddGroup1Participant(null!, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_2_participant_countryId_arg()
        {
            // Act
            Action act = () => Contest.CreateStockholmFormat()
                .AddGroup2Participant(null!, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idGenerator_arg()
        {
            // Act
            Action act = () => Contest.CreateStockholmFormat()
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idGenerator')", exception.Message);
        }

        private sealed class DummyContestIdGenerator : IContestIdGenerator
        {
            public ContestId Generate() => FixedContestId;
        }
    }
}
