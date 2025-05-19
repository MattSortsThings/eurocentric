using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Contests.TestUtils;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Contests;

public sealed class StockholmFormatContestTests : UnitTestBase
{
    private static StockholmFormatContest CreateContestWithDefaultValues()
    {
        ContestId id = ContestId.FromValue(Guid.Parse("ca62f352-2283-4e5d-bf07-97749cb4016e"));
        ContestYear year = ContestYear.FromValue(2016).Value;
        CityName cityName = CityName.FromValue("CityName").Value;

        ErrorOr<ActName> fixedActName = ActName.FromValue("ActName");
        ErrorOr<SongTitle> fixedSongTitle = SongTitle.FromValue("SongTitle");

        List<Participant> participants =
        [
            Participant.CreateInGroupOne(CountryIds.At, fixedActName, fixedSongTitle).Value,
            Participant.CreateInGroupOne(CountryIds.Be, fixedActName, fixedSongTitle).Value,
            Participant.CreateInGroupOne(CountryIds.Cz, fixedActName, fixedSongTitle).Value,
            Participant.CreateInGroupTwo(CountryIds.De, fixedActName, fixedSongTitle).Value,
            Participant.CreateInGroupTwo(CountryIds.Es, fixedActName, fixedSongTitle).Value,
            Participant.CreateInGroupTwo(CountryIds.Fi, fixedActName, fixedSongTitle).Value
        ];

        return new StockholmFormatContest(id, year, cityName, participants);
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
            StockholmFormatContest sut = CreateContestWithDefaultValues();

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
            StockholmFormatContest sut = CreateContestWithDefaultValues();

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
            StockholmFormatContest sut = CreateContestWithDefaultValues();
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
            StockholmFormatContest sut = CreateContestWithDefaultValues();

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
            StockholmFormatContest sut = CreateContestWithDefaultValues();
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
            StockholmFormatContest sut = CreateContestWithDefaultValues();

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
            StockholmFormatContest sut = CreateContestWithDefaultValues();

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
            StockholmFormatContest sut = CreateContestWithDefaultValues();
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
            StockholmFormatContest sut = CreateContestWithDefaultValues();

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
            StockholmFormatContest sut = CreateContestWithDefaultValues();

            // Act
            Action act = () => sut.RemoveMemo(FixedBroadcastId);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemos collection contains no item with the provided BroadcastId value.", exception.Message);

            Assert.Empty(sut.BroadcastMemos);
        }
    }
}
