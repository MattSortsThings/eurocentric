using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Aggregates.Utils;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests;

public static class StockholmFormatContestTests
{
    public sealed class AddChildBroadcastMethod : UnitTest
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("37c2b6a4-a81b-4395-808a-4bc74ecc5f60"));

        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2025);

        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");

        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");

        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

        private static StockholmFormatContest CreateContest()
        {
            (CountryId at,
                CountryId be,
                CountryId cz,
                CountryId dk,
                CountryId ee,
                CountryId fi) = new SixCountryIds();

            return StockholmFormatContest.Create()
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(at, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(() => FixedContestId)
                .Then(contest => (StockholmFormatContest)contest).Value;
        }

        [Fact]
        public void Should_add_broadcast_memo_and_set_Completed_to_false()
        {
            // Arrange
            StockholmFormatContest sut = CreateContest();

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("d2b2e8f2-983b-43b2-a4a3-5b3f3cc44b2e"));

            // Assert
            Assert.False(sut.Completed);
            Assert.Empty(sut.ChildBroadcasts);

            // Act;
            sut.AddChildBroadcast(broadcastId, ContestStage.GrandFinal);

            // Assert
            Assert.False(sut.Completed);

            BroadcastMemo singleChildBroadcast = Assert.Single(sut.ChildBroadcasts);
            Assert.Equal(new BroadcastMemo(broadcastId, ContestStage.GrandFinal), singleChildBroadcast);
        }

        [Fact]
        public void Should_throw_given_non_unique_broadcastId_arg()
        {
            // Arrange
            StockholmFormatContest sut = CreateContest();

            BroadcastId sharedBroadcastId = BroadcastId.FromValue(Guid.Parse("d2b2e8f2-983b-43b2-a4a3-5b3f3cc44b2e"));
            sut.AddChildBroadcast(sharedBroadcastId, ContestStage.GrandFinal);

            IReadOnlyList<BroadcastMemo> initialChildBroadcasts = sut.ChildBroadcasts;

            // Act
            Action act = () => sut.AddChildBroadcast(sharedBroadcastId, ContestStage.SemiFinal1);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("Child broadcast exists with provided broadcast ID value. (Parameter 'broadcastId')",
                exception.Message);

            Assert.False(sut.Completed);
            Assert.Equal(initialChildBroadcasts, sut.ChildBroadcasts);
        }

        [Fact]
        public void Should_throw_given_non_unique_contestStatus_arg()
        {
            // Arrange
            StockholmFormatContest sut = CreateContest();

            BroadcastId broadcastId1Of2 = BroadcastId.FromValue(Guid.Parse("d2b2e8f2-983b-43b2-a4a3-5b3f3cc44b2e"));
            BroadcastId broadcastId2Of2 = BroadcastId.FromValue(Guid.Parse("7225f657-e41a-4ef9-9599-1038bbe2f202"));

            sut.AddChildBroadcast(broadcastId1Of2, ContestStage.GrandFinal);

            IReadOnlyList<BroadcastMemo> initialChildBroadcasts = sut.ChildBroadcasts;

            // Act
            Action act = () => sut.AddChildBroadcast(broadcastId2Of2, ContestStage.GrandFinal);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("Child broadcast exists with provided contest stage value. (Parameter 'contestStage')",
                exception.Message);

            Assert.False(sut.Completed);
            Assert.Equal(initialChildBroadcasts, sut.ChildBroadcasts);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            StockholmFormatContest sut = CreateContest();

            // Act
            Action act = () => sut.AddChildBroadcast(null!, ContestStage.GrandFinal);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);

            Assert.False(sut.Completed);
            Assert.Empty(sut.ChildBroadcasts);
        }
    }

    public sealed class CreateSemiFinal1ChildBroadcastMethod : UnitTest
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("37c2b6a4-a81b-4395-808a-4bc74ecc5f60"));

        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2025);

        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");

        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");

        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

        private static Action<Competitor> AssertCorrectCompetitor(CountryId competingCountryId,
            int finishingPosition = 0,
            int runningOrderPosition = 0)
        {
            CountryId expectedCompetingCountryId = competingCountryId;
            int expectedFinishingPosition = finishingPosition;
            int expectedRunningOrderPosition = runningOrderPosition;

            return ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(expectedFinishingPosition, competitor.FinishingPosition);
                Assert.Equal(expectedRunningOrderPosition, competitor.RunningOrderPosition);
                Assert.Equal(expectedCompetingCountryId, competitor.CompetingCountryId);
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            };
        }

        private static StockholmFormatContest CreateContest(CountryId[]? group2CountryIds = null,
            CountryId[]? group1CountryIds = null,
            int contestYear = 0)
        {
            ContestBuilder contestBuilder = StockholmFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(contestYear))
                .WithCityName(ArbitraryCityName);

            foreach (CountryId countryId in group1CountryIds ?? Enumerable.Empty<CountryId>())
            {
                contestBuilder.AddGroup1Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
            }

            foreach (CountryId countryId in group2CountryIds ?? Enumerable.Empty<CountryId>())
            {
                contestBuilder.AddGroup2Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
            }

            return contestBuilder.Build(() => FixedContestId).Value as StockholmFormatContest
                   ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_return_SemiFinal1_Broadcast_with_instance_ID_as_ParentContestId_and_false_Completed()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(broadcastId, broadcast.Id);
            Assert.Equal(sut.Id, broadcast.ParentContestId);
            Assert.Equal(ContestStage.SemiFinal1, broadcast.ContestStage);
            Assert.False(broadcast.Completed);
        }

        [Fact]
        public void Should_return_Broadcast_with_Competitors_in_given_order()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([czId, atId, beId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Collection(broadcast.Competitors,
                AssertCorrectCompetitor(czId, runningOrderPosition: 1, finishingPosition: 1),
                AssertCorrectCompetitor(atId, runningOrderPosition: 2, finishingPosition: 2),
                AssertCorrectCompetitor(beId, runningOrderPosition: 3, finishingPosition: 3));
        }

        [Fact]
        public void Should_return_Broadcast_with_Jury_for_every_group_1_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(new List<CountryId> { atId, beId, czId }, broadcast.Juries.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Juries, voter => Assert.False(voter.PointsAwarded));
        }

        [Fact]
        public void Should_return_Broadcast_with_Televote_for_every_group_1_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(new List<CountryId> { atId, beId, czId },
                broadcast.Televotes.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Televotes, voter => Assert.False(voter.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_when_broadcastDate_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithCompetingCountryIds([atId, beId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Broadcast date not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_competingCountryIds_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Competing country IDs not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_instance_has_SemiFinal1_ChildBroadcast()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));
            BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("77e1ad4e-57ec-482c-a5d0-d01679de611a"));

            sut.AddChildBroadcast(existingBroadcastId, ContestStage.SemiFinal1);

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast contest stage conflict", firstError.Code);
            Assert.Equal("A child broadcast already exists for the contest with the provided contest stage.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal1" });
        }

        [Theory]
        [InlineData("2016-01-01")]
        [InlineData("2024-12-31")]
        [InlineData("2026-01-01")]
        [InlineData("2049-12-31")]
        public void Should_return_Errors_given_broadcastDate_arg_outside_instance_ContestYear(string broadcastDateValue)
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = DateOnly.ParseExact(broadcastDateValue, "yyyy-MM-dd");

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast date out of range", firstError.Code);
            Assert.Equal("A broadcast's date must be in the same year as its parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "broadcastDate", Value: string s } && s == broadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_group_2_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId, fiId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Ineligible competing country ID", firstError.Code);
            Assert.Equal("Competing country ID matches ineligible participant in parent contest given contest stage.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryId", Value: Guid g } && g == fiId.Value);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal1" });
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_no_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId, gbId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Orphan competing country ID", firstError.Code);
            Assert.Equal("Competing country ID has no matching participant in parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryId", Value: Guid g } && g == gbId.Value);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_competing_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId, atId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing country IDs", firstError.Code);
            Assert.Equal("Each competitor in a broadcast must have a competing country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_two_competing_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast size", firstError.Code);
            Assert.Equal("A broadcast must have at least 2 competitors.", firstError.Description);
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            Action act = () => sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds(null!)
                .Build(() => broadcastId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            // Act
            Action act = () => sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId])
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }

    public sealed class CreateSemiFinal2ChildBroadcastMethod : UnitTest
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("37c2b6a4-a81b-4395-808a-4bc74ecc5f60"));

        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2025);

        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");

        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");

        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

        private static Action<Competitor> AssertCorrectCompetitor(CountryId competingCountryId,
            int finishingPosition = 0,
            int runningOrderPosition = 0)
        {
            CountryId expectedCompetingCountryId = competingCountryId;
            int expectedFinishingPosition = finishingPosition;
            int expectedRunningOrderPosition = runningOrderPosition;

            return ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(expectedFinishingPosition, competitor.FinishingPosition);
                Assert.Equal(expectedRunningOrderPosition, competitor.RunningOrderPosition);
                Assert.Equal(expectedCompetingCountryId, competitor.CompetingCountryId);
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            };
        }

        private static StockholmFormatContest CreateContest(CountryId[]? group2CountryIds = null,
            CountryId[]? group1CountryIds = null,
            int contestYear = 0)
        {
            ContestBuilder contestBuilder = StockholmFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(contestYear))
                .WithCityName(ArbitraryCityName);

            foreach (CountryId countryId in group1CountryIds ?? Enumerable.Empty<CountryId>())
            {
                contestBuilder.AddGroup1Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
            }

            foreach (CountryId countryId in group2CountryIds ?? Enumerable.Empty<CountryId>())
            {
                contestBuilder.AddGroup2Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
            }

            return contestBuilder.Build(() => FixedContestId).Value as StockholmFormatContest
                   ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_return_SemiFinal2_Broadcast_with_instance_ID_as_ParentContestId_and_false_Completed()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(broadcastId, broadcast.Id);
            Assert.Equal(sut.Id, broadcast.ParentContestId);
            Assert.Equal(ContestStage.SemiFinal2, broadcast.ContestStage);
            Assert.False(broadcast.Completed);
        }

        [Fact]
        public void Should_return_Broadcast_with_Competitors_in_given_order()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([fiId, dkId, eeId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Collection(broadcast.Competitors,
                AssertCorrectCompetitor(fiId, runningOrderPosition: 1, finishingPosition: 1),
                AssertCorrectCompetitor(dkId, runningOrderPosition: 2, finishingPosition: 2),
                AssertCorrectCompetitor(eeId, runningOrderPosition: 3, finishingPosition: 3));
        }

        [Fact]
        public void Should_return_Broadcast_with_Jury_for_every_group_2_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(new List<CountryId> { dkId, eeId, fiId }, broadcast.Juries.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Juries, voter => Assert.False(voter.PointsAwarded));
        }

        [Fact]
        public void Should_return_Broadcast_with_Televote_for_every_group_2_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(new List<CountryId> { dkId, eeId, fiId },
                broadcast.Televotes.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Televotes, voter => Assert.False(voter.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_when_broadcastDate_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithCompetingCountryIds([dkId, eeId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Broadcast date not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_competingCountryIds_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Competing country IDs not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_instance_has_SemiFinal2_ChildBroadcast()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));
            BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("77e1ad4e-57ec-482c-a5d0-d01679de611a"));

            sut.AddChildBroadcast(existingBroadcastId, ContestStage.SemiFinal2);

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast contest stage conflict", firstError.Code);
            Assert.Equal("A child broadcast already exists for the contest with the provided contest stage.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal2" });
        }

        [Theory]
        [InlineData("2016-01-01")]
        [InlineData("2024-12-31")]
        [InlineData("2026-01-01")]
        [InlineData("2049-12-31")]
        public void Should_return_Errors_given_broadcastDate_arg_outside_instance_ContestYear(string broadcastDateValue)
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = DateOnly.ParseExact(broadcastDateValue, "yyyy-MM-dd");

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast date out of range", firstError.Code);
            Assert.Equal("A broadcast's date must be in the same year as its parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "broadcastDate", Value: string s } && s == broadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_group_1_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId, czId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Ineligible competing country ID", firstError.Code);
            Assert.Equal("Competing country ID matches ineligible participant in parent contest given contest stage.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryId", Value: Guid g } && g == czId.Value);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "SemiFinal2" });
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_no_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId, gbId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Orphan competing country ID", firstError.Code);
            Assert.Equal("Competing country ID has no matching participant in parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryId", Value: Guid g } && g == gbId.Value);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_competing_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId, dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing country IDs", firstError.Code);
            Assert.Equal("Each competitor in a broadcast must have a competing country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_two_competing_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast size", firstError.Code);
            Assert.Equal("A broadcast must have at least 2 competitors.", firstError.Description);
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            Action act = () => sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds(null!)
                .Build(() => broadcastId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            // Act
            Action act = () => sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([dkId, eeId])
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }

    public sealed class CreateGrandFinalChildBroadcastMethod : UnitTest
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("37c2b6a4-a81b-4395-808a-4bc74ecc5f60"));

        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2025);

        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");

        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");

        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

        private static Action<Competitor> AssertCorrectCompetitor(CountryId competingCountryId,
            int finishingPosition = 0,
            int runningOrderPosition = 0)
        {
            CountryId expectedCompetingCountryId = competingCountryId;
            int expectedFinishingPosition = finishingPosition;
            int expectedRunningOrderPosition = runningOrderPosition;

            return ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(expectedFinishingPosition, competitor.FinishingPosition);
                Assert.Equal(expectedRunningOrderPosition, competitor.RunningOrderPosition);
                Assert.Equal(expectedCompetingCountryId, competitor.CompetingCountryId);
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            };
        }

        private static StockholmFormatContest CreateContest(CountryId[]? group2CountryIds = null,
            CountryId[]? group1CountryIds = null,
            int contestYear = 0)
        {
            ContestBuilder contestBuilder = StockholmFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(contestYear))
                .WithCityName(ArbitraryCityName);

            foreach (CountryId countryId in group1CountryIds ?? Enumerable.Empty<CountryId>())
            {
                contestBuilder.AddGroup1Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
            }

            foreach (CountryId countryId in group2CountryIds ?? Enumerable.Empty<CountryId>())
            {
                contestBuilder.AddGroup2Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
            }

            return contestBuilder.Build(() => FixedContestId).Value as StockholmFormatContest
                   ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_return_GrandFinal_Broadcast_with_instance_ID_as_ParentContestId_and_false_Completed()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(broadcastId, broadcast.Id);
            Assert.Equal(sut.Id, broadcast.ParentContestId);
            Assert.Equal(ContestStage.GrandFinal, broadcast.ContestStage);
            Assert.False(broadcast.Completed);
        }

        [Fact]
        public void Should_return_Broadcast_with_Competitors_in_given_order()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([czId, eeId, fiId, atId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Collection(broadcast.Competitors,
                AssertCorrectCompetitor(czId, runningOrderPosition: 1, finishingPosition: 1),
                AssertCorrectCompetitor(eeId, runningOrderPosition: 2, finishingPosition: 2),
                AssertCorrectCompetitor(fiId, runningOrderPosition: 3, finishingPosition: 3),
                AssertCorrectCompetitor(atId, runningOrderPosition: 4, finishingPosition: 4));
        }

        [Fact]
        public void Should_return_Broadcast_with_Jury_for_every_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(new List<CountryId> { atId, beId, czId, dkId, eeId, fiId },
                broadcast.Juries.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Juries, voter => Assert.False(voter.PointsAwarded));
        }

        [Fact]
        public void Should_return_Broadcast_with_Televote_for_every_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(new List<CountryId> { atId, beId, czId, dkId, eeId, fiId },
                broadcast.Televotes.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Televotes, voter => Assert.False(voter.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_when_broadcastDate_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithCompetingCountryIds([atId, dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Broadcast date not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_competingCountryIds_not_provided()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Unexpected, firstError.Type);
            Assert.Equal("Competing country IDs not provided", firstError.Code);
        }

        [Fact]
        public void Should_return_Errors_when_instance_has_GrandFinal_ChildBroadcast()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));
            BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("77e1ad4e-57ec-482c-a5d0-d01679de611a"));

            sut.AddChildBroadcast(existingBroadcastId, ContestStage.GrandFinal);

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast contest stage conflict", firstError.Code);
            Assert.Equal("A child broadcast already exists for the contest with the provided contest stage.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: "GrandFinal" });
        }

        [Theory]
        [InlineData("2016-01-01")]
        [InlineData("2024-12-31")]
        [InlineData("2026-01-01")]
        [InlineData("2049-12-31")]
        public void Should_return_Errors_given_broadcastDate_arg_outside_instance_ContestYear(string broadcastDateValue)
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = DateOnly.ParseExact(broadcastDateValue, "yyyy-MM-dd");

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, dkId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast date out of range", firstError.Code);
            Assert.Equal("A broadcast's date must be in the same year as its parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata,
                kvp => kvp is { Key: "broadcastDate", Value: string s } && s == broadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_no_Participant()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId,
                CountryId gbId) = new SevenCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, dkId, gbId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Orphan competing country ID", firstError.Code);
            Assert.Equal("Competing country ID has no matching participant in parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "countryId", Value: Guid g } && g == gbId.Value);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_competing_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, dkId, atId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing country IDs", firstError.Code);
            Assert.Equal("Each competitor in a broadcast must have a competing country ID referencing a different country.",
                firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_two_competing_country_IDs()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId])
                .Build(() => broadcastId);

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast size", firstError.Code);
            Assert.Equal("A broadcast must have at least 2 competitors.", firstError.Description);
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("ec6292fb-5802-4f8b-a459-33085f4b9684"));

            // Act
            Action act = () => sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds(null!)
                .Build(() => broadcastId);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idProvider_arg()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            StockholmFormatContest sut = CreateContest(contestYear: 2025,
                group1CountryIds: [atId, beId, czId],
                group2CountryIds: [dkId, eeId, fiId]);

            DateOnly broadcastDate = new(2025, 5, 1);

            // Act
            Action act = () => sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds([atId, beId])
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idProvider')", exception.Message);
        }
    }

    public sealed class FluentBuilder : UnitTest
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("37c2b6a4-a81b-4395-808a-4bc74ecc5f60"));

        private static readonly ErrorOr<ContestYear> ArbitraryContestYear = ContestYear.FromValue(2025);

        private static readonly ErrorOr<CityName> ArbitraryCityName = CityName.FromValue("CityName");

        private static readonly ErrorOr<ActName> ArbitraryActName = ActName.FromValue("ActName");

        private static readonly ErrorOr<SongTitle> ArbitrarySongTitle = SongTitle.FromValue("SongTitle");

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

        [Fact]
        public void Should_return_new_StockholmFormatContest_given_valid_args_scenario_1()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(2022))
                .WithCityName(CityName.FromValue("Turin"))
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

            StockholmFormatContest createdContest = Assert.IsType<StockholmFormatContest>(contest);

            Assert.Equal(FixedContestId, createdContest.Id);
            Assert.Equal(2022, createdContest.ContestYear.Value);
            Assert.Equal("Turin", createdContest.CityName.Value);
            Assert.Equal(ContestFormat.Stockholm, createdContest.ContestFormat);
            Assert.False(createdContest.Completed);
            Assert.Empty(createdContest.ChildBroadcasts);

            Assert.Collection(createdContest.Participants,
                AssertCorrectGroup1Participant(atId, "AT Act", "AT Song"),
                AssertCorrectGroup1Participant(beId, "BE Act", "BE Song"),
                AssertCorrectGroup1Participant(czId, "CZ Act", "CZ Song"),
                AssertCorrectGroup2Participant(dkId, "DK Act", "DK Song"),
                AssertCorrectGroup2Participant(eeId, "EE Act", "EE Song"),
                AssertCorrectGroup2Participant(fiId, "FI Act", "FI Song"));
        }

        [Fact]
        public void Should_return_new_StockholmFormatContest_given_valid_args_scenario_2()
        {
            // Arrange
            (CountryId atId,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(2016))
                .WithCityName(CityName.FromValue("Stockholm"))
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

            StockholmFormatContest createdContest = Assert.IsType<StockholmFormatContest>(contest);

            Assert.Equal(FixedContestId, createdContest.Id);
            Assert.Equal(2016, createdContest.ContestYear.Value);
            Assert.Equal("Stockholm", createdContest.CityName.Value);
            Assert.Equal(ContestFormat.Stockholm, createdContest.ContestFormat);
            Assert.False(createdContest.Completed);
            Assert.Empty(createdContest.ChildBroadcasts);

            Assert.Collection(createdContest.Participants,
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
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
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
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .WithContestYear(ArbitraryContestYear)
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
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .Build(() => FixedContestId);

            (bool isError, Contest? contest, Error firstError) =
                (errorsOrContest.IsError, errorsOrContest.Value, errorsOrContest.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Stockholm format participant groups", firstError.Code);
            Assert.Equal("A Stockholm format contest must have no group 0 participants, " +
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
                CountryId fiId) = new SixCountryIds();

            const int contestYear = 0;

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .WithContestYear(ContestYear.FromValue(contestYear))
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
                CountryId fiId) = new SixCountryIds();

            const string cityName = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .WithCityName(CityName.FromValue(cityName))
                .WithContestYear(ArbitraryContestYear)
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
                CountryId fiId) = new SixCountryIds();

            const string actName = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup1Participant(atId, ActName.FromValue(actName), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
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
                CountryId fiId) = new SixCountryIds();

            const string actName = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup2Participant(fiId, ActName.FromValue(actName), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
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
                CountryId fiId) = new SixCountryIds();

            const string songTitle = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup1Participant(atId, ArbitraryActName, SongTitle.FromValue(songTitle))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
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
                CountryId fiId) = new SixCountryIds();

            const string songTitle = " ";

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup2Participant(fiId, ArbitraryActName, SongTitle.FromValue(songTitle))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
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
        public void Should_return_Errors_given_duplicate_group_1_and_group_2_participating_country_IDs()
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
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup1Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
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
        public void Should_return_Errors_given_duplicate_group_1_participating_country_IDs()
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
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup1Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
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
        public void Should_return_Errors_given_duplicate_group_2_participating_country_IDs()
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
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup2Participant(gbId, ArbitraryActName, ArbitrarySongTitle)
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
        public void Should_return_Errors_given_non_zero_group_0_participants()
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
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup0Participant(gbId)
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
            Assert.Equal("Illegal Stockholm format participant groups", firstError.Code);
            Assert.Equal("A Stockholm format contest must have no group 0 participants, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_three_group_1_participants()
        {
            // Arrange
            (CountryId _,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup1Participant(beId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(czId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
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
            Assert.Equal("Illegal Stockholm format participant groups", firstError.Code);
            Assert.Equal("A Stockholm format contest must have no group 0 participants, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_three_group_2_participants()
        {
            // Arrange
            (CountryId _,
                CountryId beId,
                CountryId czId,
                CountryId dkId,
                CountryId eeId,
                CountryId fiId) = new SixCountryIds();

            // Act
            ErrorOr<Contest> errorsOrContest = StockholmFormatContest.Create()
                .AddGroup2Participant(eeId, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(fiId, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
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
            Assert.Equal("Illegal Stockholm format participant groups", firstError.Code);
            Assert.Equal("A Stockholm format contest must have no group 0 participants, " +
                         "at least three group 1 participants, " +
                         "and at least three group 2 participants.", firstError.Description);
        }
    }
}
