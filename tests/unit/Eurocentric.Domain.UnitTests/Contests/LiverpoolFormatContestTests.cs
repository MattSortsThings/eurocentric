using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Contests.TestUtils;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Contests;

public sealed class LiverpoolFormatContestTests : UnitTestBase
{
    private static LiverpoolFormatContest CreateContestWithDefaultValues()
    {
        ContestId fixedId = ContestId.FromValue(Guid.Parse("ca62f352-2283-4e5d-bf07-97749cb4016e"));
        ErrorOr<ActName> fixedActName = ActName.FromValue("ActName");
        ErrorOr<SongTitle> fixedSongTitle = SongTitle.FromValue("SongTitle");

        return (LiverpoolFormatContest)LiverpoolFormatContest.Create()
            .WithYear(ContestYear.FromValue(2025))
            .WithCityName(CityName.FromValue("CityName"))
            .WithGroupZeroParticipant(CountryIds.Xx)
            .WithGroupOneParticipant(CountryIds.At, fixedActName, fixedSongTitle)
            .WithGroupOneParticipant(CountryIds.Be, fixedActName, fixedSongTitle)
            .WithGroupOneParticipant(CountryIds.Cz, fixedActName, fixedSongTitle)
            .WithGroupTwoParticipant(CountryIds.De, fixedActName, fixedSongTitle)
            .WithGroupTwoParticipant(CountryIds.Es, fixedActName, fixedSongTitle)
            .WithGroupTwoParticipant(CountryIds.Fi, fixedActName, fixedSongTitle)
            .Build(() => fixedId)
            .Value;
    }

    public sealed class AddMemoMethod : UnitTestBase
    {
        private const ContestStage FixedContestStage = ContestStage.SemiFinal2;
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("785913e9-dad3-4504-90aa-34e1175fb02e"));

        [Fact]
        public void Should_add_broadcast_memo_with_initialized_status()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();

            // Assert
            Assert.Empty(sut.BroadcastMemos);

            // Act
            sut.AddMemo(FixedBroadcastId, FixedContestStage);

            // Assert
            Assert.Single(sut.BroadcastMemos,
                new BroadcastMemo(FixedBroadcastId, FixedContestStage, BroadcastStatus.Initialized));
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();

            // Assert
            Assert.Empty(sut.BroadcastMemos);

            // Act
            Action act = () => sut.AddMemo(null!, FixedContestStage);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_when_existing_broadcast_memo_has_provided_broadcast_ID()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();
            sut.AddMemo(FixedBroadcastId, ContestStage.SemiFinal1);

            // Assert
            Assert.Single(sut.BroadcastMemos);

            // Act
            Action act = () => sut.AddMemo(FixedBroadcastId, ContestStage.GrandFinal);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains an item with the provided BroadcastId value.", exception.Message);

            Assert.Single(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_when_existing_broadcast_memo_has_provided_contest_stage()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();

            BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("6076469b-8a95-4e3d-82ed-76c111984458"));
            BroadcastId newBroadcastId = BroadcastId.FromValue(Guid.Parse("ac73ac4c-187e-4d8a-b3df-6399bdd52ad5"));

            sut.AddMemo(existingBroadcastId, FixedContestStage);

            // Assert
            Assert.Single(sut.BroadcastMemos);

            // Act
            Action act = () => sut.AddMemo(newBroadcastId, FixedContestStage);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains an item with the provided ContestStage value.", exception.Message);

            Assert.Single(sut.BroadcastMemos);
        }
    }

    public sealed class ReplaceMemoMethod : UnitTestBase
    {
        private const ContestStage FixedContestStage = ContestStage.SemiFinal2;
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("785913e9-dad3-4504-90aa-34e1175fb02e"));

        [Fact]
        public void Should_replace_broadcast_memo_with_new_memo_having_provided_status()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();
            sut.AddMemo(FixedBroadcastId, FixedContestStage);

            // Assert
            Assert.Single(sut.BroadcastMemos,
                new BroadcastMemo(FixedBroadcastId, FixedContestStage, BroadcastStatus.Initialized));

            // Act
            sut.ReplaceMemo(FixedBroadcastId, BroadcastStatus.InProgress);

            // Assert
            Assert.Single(sut.BroadcastMemos,
                new BroadcastMemo(FixedBroadcastId, FixedContestStage, BroadcastStatus.InProgress));
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();

            // Act
            Action act = () => sut.ReplaceMemo(null!, BroadcastStatus.InProgress);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_when_no_existing_broadcast_memo_has_provided_broadcast_ID()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();

            // Act
            Action act = () => sut.ReplaceMemo(FixedBroadcastId, BroadcastStatus.InProgress);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains no item with the provided BroadcastId value.", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }
    }

    public sealed class RemoveMemoMethod : UnitTestBase
    {
        private const ContestStage FixedContestStage = ContestStage.SemiFinal2;
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("785913e9-dad3-4504-90aa-34e1175fb02e"));

        [Fact]
        public void Should_remove_broadcast_memo_having_provided_contest_ID()
        {
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();
            sut.AddMemo(FixedBroadcastId, FixedContestStage);

            // Assert
            Assert.Single(sut.BroadcastMemos,
                new BroadcastMemo(FixedBroadcastId, FixedContestStage, BroadcastStatus.Initialized));

            // Act
            sut.RemoveMemo(FixedBroadcastId);

            // Assert
            Assert.Empty(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();

            // Act
            Action act = () => sut.RemoveMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }

        [Fact]
        public void Should_throw_when_no_existing_broadcast_memo_has_provided_broadcast_ID()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateContestWithDefaultValues();

            // Act
            Action act = () => sut.RemoveMemo(FixedBroadcastId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains no item with the provided BroadcastId value.", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }
    }

    public sealed class FluentBuilder : UnitTestBase
    {
        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2025);
        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");
        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");
        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("989c773a-0306-4136-813d-c10d8de2935b"));

        [Theory]
        [InlineData(2023, "Liverpool")]
        [InlineData(2025, "Basel")]
        public void Should_create_contest_with_provided_values_and_empty_broadcast_memos(int contestYear, string cityName)
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithYear(ContestYear.FromValue(contestYear))
                .WithCityName(CityName.FromValue(cityName))
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ActName.FromValue("AT Act"), SongTitle.FromValue("AT Song"))
                .WithGroupOneParticipant(CountryIds.De, ActName.FromValue("DE Act"), SongTitle.FromValue("DE Song"))
                .WithGroupOneParticipant(CountryIds.Cz, ActName.FromValue("CZ Act"), SongTitle.FromValue("CZ Song"))
                .WithGroupTwoParticipant(CountryIds.Es, ActName.FromValue("ES Act"), SongTitle.FromValue("ES Song"))
                .WithGroupTwoParticipant(CountryIds.Fi, ActName.FromValue("FI Act"), SongTitle.FromValue("FI Song"))
                .WithGroupTwoParticipant(CountryIds.Be, ActName.FromValue("BE Act"), SongTitle.FromValue("BE Song"))
                .Build(() => FixedContestId);

            (bool isError, Contest result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.IsType<LiverpoolFormatContest>(result);

            Assert.Equal(FixedContestId, result.Id);
            Assert.Equal(contestYear, result.Year.Value);
            Assert.Equal(cityName, result.CityName.Value);
            Assert.Equal(ContestFormat.Liverpool, result.Format);
            Assert.Equal(ContestStatus.Initialized, result.Status);
            Assert.Empty(result.BroadcastMemos);
            Assert.Collection(result.Participants, p =>
            {
                Assert.Equal(ParticipantGroup.Zero, p.Group);
                Assert.Equal(CountryIds.Xx, p.ParticipatingCountryId);
                Assert.Null(p.ActName);
                Assert.Null(p.SongTitle);
            }, p =>
            {
                Assert.Equal(ParticipantGroup.One, p.Group);
                Assert.Equal(CountryIds.At, p.ParticipatingCountryId);
                Assert.NotNull(p.ActName);
                Assert.Equal("AT Act", p.ActName.Value);
                Assert.NotNull(p.SongTitle);
                Assert.Equal("AT Song", p.SongTitle.Value);
            }, p =>
            {
                Assert.Equal(ParticipantGroup.One, p.Group);
                Assert.Equal(CountryIds.Cz, p.ParticipatingCountryId);
                Assert.NotNull(p.ActName);
                Assert.Equal("CZ Act", p.ActName.Value);
                Assert.NotNull(p.SongTitle);
                Assert.Equal("CZ Song", p.SongTitle.Value);
            }, p =>
            {
                Assert.Equal(ParticipantGroup.One, p.Group);
                Assert.Equal(CountryIds.De, p.ParticipatingCountryId);
                Assert.NotNull(p.ActName);
                Assert.Equal("DE Act", p.ActName.Value);
                Assert.NotNull(p.SongTitle);
                Assert.Equal("DE Song", p.SongTitle.Value);
            }, p =>
            {
                Assert.Equal(ParticipantGroup.Two, p.Group);
                Assert.Equal(CountryIds.Be, p.ParticipatingCountryId);
                Assert.NotNull(p.ActName);
                Assert.Equal("BE Act", p.ActName.Value);
                Assert.NotNull(p.SongTitle);
                Assert.Equal("BE Song", p.SongTitle.Value);
            }, p =>
            {
                Assert.Equal(ParticipantGroup.Two, p.Group);
                Assert.Equal(CountryIds.Es, p.ParticipatingCountryId);
                Assert.NotNull(p.ActName);
                Assert.Equal("ES Act", p.ActName.Value);
                Assert.NotNull(p.SongTitle);
                Assert.Equal("ES Song", p.SongTitle.Value);
            }, p =>
            {
                Assert.Equal(ParticipantGroup.Two, p.Group);
                Assert.Equal(CountryIds.Fi, p.ParticipatingCountryId);
                Assert.NotNull(p.ActName);
                Assert.Equal("FI Act", p.ActName.Value);
                Assert.NotNull(p.SongTitle);
                Assert.Equal("FI Song", p.SongTitle.Value);
            });
        }

        [Fact]
        public void Should_return_errors_given_illegal_contest_year_value()
        {
            // Arrange
            ErrorOr<ContestYear> illegalContestYear = ContestYear.FromValue(0);

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithYear(illegalContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal contest year value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_illegal_city_name_value()
        {
            // Arrange
            ErrorOr<CityName> illegalCityName = CityName.FromValue(string.Empty);

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithCityName(illegalCityName)
                .WithYear(ArbitraryContestYear)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_1_participant_with_illegal_act_name_value()
        {
            // Arrange
            ErrorOr<ActName> illegalActName = ActName.FromValue(string.Empty);

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.At, illegalActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_1_participant_with_illegal_song_title_value()
        {
            // Arrange
            ErrorOr<SongTitle> illegalSongTitle = SongTitle.FromValue(string.Empty);

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, illegalSongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_2_participant_with_illegal_act_name_value()
        {
            // Arrange
            ErrorOr<ActName> illegalActName = ActName.FromValue(string.Empty);

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(CountryIds.Fi, illegalActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_2_participant_with_illegal_song_title_value()
        {
            // Arrange
            ErrorOr<SongTitle> illegalSongTitle = SongTitle.FromValue(string.Empty);

            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, illegalSongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_0_and_group_1_participants_with_same_participating_country_ID()
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
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_0_and_group_2_participants_with_same_participating_country_ID()
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
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_1_and_group_2_participants_with_same_participating_country_ID()
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
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_1_participants_with_same_participating_country_ID()
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
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_group_2_participants_with_same_participating_country_ID()
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
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Duplicate participating country IDs", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
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
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_multiple_group_0_participants()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupZeroParticipant(CountryIds.Gb)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_3_group_1_participants()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            (bool isError, Contest result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_3_group_2_participants()
        {
            // Act
            ErrorOr<Contest> errorsOrResult = LiverpoolFormatContest.Create()
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
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

            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal(ErrorType.Failure, firstError.Type);
        }

        [Fact]
        public void Should_throw_given_null_group_0_participant_countryId_arg()
        {
            // Act
            Action act = () => LiverpoolFormatContest.Create()
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(null!)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_1_participant_countryId_arg()
        {
            // Act
            Action act = () => LiverpoolFormatContest.Create()
                .WithYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .WithGroupZeroParticipant(CountryIds.Xx)
                .WithGroupOneParticipant(CountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(CountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupOneParticipant(null!, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.De, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_2_participant_countryId_arg()
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
                .WithGroupTwoParticipant(null!, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
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
                .WithGroupTwoParticipant(CountryIds.Es, ArbitraryActName, ArbitrarySongTitle)
                .WithGroupTwoParticipant(CountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }
}
