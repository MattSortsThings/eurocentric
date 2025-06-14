using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.UnitTests.Broadcasts;

public sealed class BroadcastTests : UnitTestBase
{
    public sealed class AwardJuryPointsMethod : UnitTestBase
    {
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("bf5a7250-3a6b-456f-a3f8-d8280851afff"));
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
        private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;
        private static readonly BroadcastDate BroadcastDate1May2025 =
            BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-01", "yyyy-mm-dd")).Value;

        [Fact]
        public void Should_update_Competitors_JuryAwards_and_FinishingPosition_values()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(1, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                Assert.Equal(1, competitor.RunningOrderPosition);
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(2, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                Assert.Equal(2, competitor.RunningOrderPosition);
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            });

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(1, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                Assert.Equal(2, competitor.RunningOrderPosition);
                Assert.Empty(competitor.TelevoteAwards);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(TestCountryIds.Cz, award.VotingCountryId);
                Assert.Equal(PointsValue.Twelve, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(2, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                Assert.Equal(1, competitor.RunningOrderPosition);
                Assert.Empty(competitor.TelevoteAwards);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(TestCountryIds.Cz, award.VotingCountryId);
                Assert.Equal(PointsValue.Ten, award.PointsValue);
            });
        }

        [Fact]
        public void Should_award_first_ten_ranked_Competitors_12_points_to_1_point_then_all_others_0_points()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz, TestCountryIds.Dk,
                    TestCountryIds.Ee, TestCountryIds.Fi)
                .WithGroup2Countries(TestCountryIds.Gb, TestCountryIds.Hr, TestCountryIds.It, TestCountryIds.Lu,
                    TestCountryIds.Mt, TestCountryIds.Nl, TestCountryIds.Pt)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz,
                    TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi,
                    TestCountryIds.Gb, TestCountryIds.Hr, TestCountryIds.It,
                    TestCountryIds.Lu, TestCountryIds.Mt, TestCountryIds.Nl)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardJuryPoints(TestCountryIds.Pt,
            [
                TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz,
                TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi,
                TestCountryIds.Gb, TestCountryIds.Hr, TestCountryIds.It,
                TestCountryIds.Lu, TestCountryIds.Mt, TestCountryIds.Nl
            ]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Twelve, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Ten, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Eight, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Dk, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Seven, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Ee, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Six, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Fi, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Five, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Gb, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Four, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Hr, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Three, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.It, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Two, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Lu, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.One, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Mt, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Zero, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Nl, competitor.CompetingCountryId);
                JuryAward award = Assert.Single(competitor.JuryAwards);
                Assert.Equal(PointsValue.Zero, award.PointsValue);
            });
        }

        [Fact]
        public void Should_set_Jury_PointsAwarded_value_to_true()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.All(sut.Juries, jury => Assert.False(jury.PointsAwarded));

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Jury singleJuryWithPointsAwarded = Assert.Single(sut.Juries, jury => jury.PointsAwarded);
            Assert.Equal(TestCountryIds.Cz, singleJuryWithPointsAwarded.VotingCountryId);
        }

        [Fact]
        public void Should_set_instance_BroadcastStatus_value_from_Initialized_to_InProgress_on_first_award()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);
        }

        [Fact]
        public void Should_set_BroadcastStatus_value_to_InProgress_if_any_Jury_or_Televote_now_has_false_PointsAwarded()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardTelevotePoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);
        }

        [Fact]
        public void Should_set_BroadcastStatus_value_to_Complete_if_every_Jury_and_Televote_now_has_true_PointsAwarded()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardTelevotePoints(TestCountryIds.At, [TestCountryIds.Be]);
            sut.AwardTelevotePoints(TestCountryIds.Be, [TestCountryIds.At]);
            sut.AwardTelevotePoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);
            sut.AwardJuryPoints(TestCountryIds.At, [TestCountryIds.Be]);
            sut.AwardJuryPoints(TestCountryIds.Be, [TestCountryIds.At]);

            // Assert
            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Completed, sut.BroadcastStatus);
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_contains_votingCountryId()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.At, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_contains_duplicate_IDs()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.At, [TestCountryIds.Be, TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_contains_ID_matching_no_Competitor()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId wrongCountryId = CountryId.FromValue(Guid.Parse("3fb1a15e-83a9-400b-82d4-880dcfd8cbc0"));

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardJuryPoints(TestCountryIds.At, [TestCountryIds.Be, wrongCountryId]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_omits_Competitor_that_is_not_Jury()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_Jury_does_not_exist()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId wrongCountryId = CountryId.FromValue(Guid.Parse("3fb1a15e-83a9-400b-82d4-880dcfd8cbc0"));

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardJuryPoints(wrongCountryId, [TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_Jury_has_true_PointsAwarded_value()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardJuryPoints(TestCountryIds.Cz, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Single(competitor.JuryAwards));
            Assert.Single(sut.Juries, jury => jury.PointsAwarded);
        }

        [Fact]
        public void Should_throw_given_null_votingCountryId_arg()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            List<CountryId> arbitraryCompetingCountryIds = [TestCountryIds.At];

            // Act
            Action act = () => sut.AwardTelevotePoints(null!, arbitraryCompetingCountryIds);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'votingCountryId')", exception.Message);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_throw_given_null_rankedCompetingCountryIds_arg()
        {
            // Arrange
            Contest contest = Contest.CreateStockholmFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId arbitraryVotingCountryId = TestCountryIds.At;

            // Act
            Action act = () => sut.AwardTelevotePoints(arbitraryVotingCountryId, null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'rankedCompetingCountryIds')", exception.Message);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }
    }

    public sealed class AwardTelevotePointsMethod : UnitTestBase
    {
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("bf5a7250-3a6b-456f-a3f8-d8280851afff"));
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
        private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;
        private static readonly BroadcastDate BroadcastDate1May2025 =
            BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-01", "yyyy-mm-dd")).Value;

        [Fact]
        public void Should_update_Competitors_TelevoteAwards_and_FinishingPosition_values()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(1, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                Assert.Equal(1, competitor.RunningOrderPosition);
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(2, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                Assert.Equal(2, competitor.RunningOrderPosition);
                Assert.Empty(competitor.JuryAwards);
                Assert.Empty(competitor.TelevoteAwards);
            });

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.Be, TestCountryIds.At]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(1, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                Assert.Equal(2, competitor.RunningOrderPosition);
                Assert.Empty(competitor.JuryAwards);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(TestCountryIds.Xx, award.VotingCountryId);
                Assert.Equal(PointsValue.Twelve, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(2, competitor.FinishingPosition);
                Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                Assert.Equal(1, competitor.RunningOrderPosition);
                Assert.Empty(competitor.JuryAwards);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(TestCountryIds.Xx, award.VotingCountryId);
                Assert.Equal(PointsValue.Ten, award.PointsValue);
            });
        }

        [Fact]
        public void Should_award_first_ten_ranked_Competitors_12_points_to_1_point_then_all_others_0_points()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz, TestCountryIds.Dk,
                    TestCountryIds.Ee, TestCountryIds.Fi)
                .WithGroup2Countries(TestCountryIds.Gb, TestCountryIds.Hr, TestCountryIds.It, TestCountryIds.Lu,
                    TestCountryIds.Mt, TestCountryIds.Nl)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz,
                    TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi,
                    TestCountryIds.Gb, TestCountryIds.Hr, TestCountryIds.It,
                    TestCountryIds.Lu, TestCountryIds.Mt, TestCountryIds.Nl)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardTelevotePoints(TestCountryIds.Xx,
            [
                TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz,
                TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi,
                TestCountryIds.Gb, TestCountryIds.Hr, TestCountryIds.It,
                TestCountryIds.Lu, TestCountryIds.Mt, TestCountryIds.Nl
            ]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Twelve, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Ten, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Eight, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Dk, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Seven, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Ee, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Six, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Fi, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Five, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Gb, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Four, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Hr, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Three, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.It, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Two, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Lu, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.One, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Mt, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Zero, award.PointsValue);
            }, ([UsedImplicitly] competitor) =>
            {
                Assert.Equal(TestCountryIds.Nl, competitor.CompetingCountryId);
                TelevoteAward award = Assert.Single(competitor.TelevoteAwards);
                Assert.Equal(PointsValue.Zero, award.PointsValue);
            });
        }

        [Fact]
        public void Should_set_Televote_PointsAwarded_value_to_true()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Televote singleTelevoteWithPointsAwarded = Assert.Single(sut.Televotes, televote => televote.PointsAwarded);
            Assert.Equal(TestCountryIds.Xx, singleTelevoteWithPointsAwarded.VotingCountryId);
        }

        [Fact]
        public void Should_set_instance_BroadcastStatus_value_from_Initialized_to_InProgress_on_first_award()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);
        }

        [Fact]
        public void Should_set_BroadcastStatus_value_to_InProgress_if_any_Jury_or_Televote_now_has_false_PointsAwarded()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardTelevotePoints(TestCountryIds.At, [TestCountryIds.Be]);

            // Assert
            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);
        }

        [Fact]
        public void Should_set_BroadcastStatus_value_to_Completed_if_every_Jury_and_Televote_now_has_true_PointsAwarded()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardTelevotePoints(TestCountryIds.At, [TestCountryIds.Be]);
            sut.AwardTelevotePoints(TestCountryIds.Be, [TestCountryIds.At]);
            sut.AwardTelevotePoints(TestCountryIds.Cz, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Completed, sut.BroadcastStatus);
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_contains_votingCountryId()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.At, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_contains_duplicate_IDs()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.At, [TestCountryIds.Be, TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_contains_ID_matching_no_Competitor()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId wrongCountryId = CountryId.FromValue(Guid.Parse("3fb1a15e-83a9-400b-82d4-880dcfd8cbc0"));

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardTelevotePoints(TestCountryIds.At, [TestCountryIds.Be, wrongCountryId]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_rankedCompetingCountryIds_omits_Competitor_that_is_not_Televote()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_Televote_does_not_exist()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId wrongCountryId = CountryId.FromValue(Guid.Parse("3fb1a15e-83a9-400b-82d4-880dcfd8cbc0"));

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.AwardTelevotePoints(wrongCountryId, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_errors_and_not_update_when_Televote_has_true_PointsAwarded_value()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Single(competitor.TelevoteAwards));
            Assert.Single(sut.Televotes, televote => televote.PointsAwarded);

            // Act
            ErrorOr<Updated> errorsOrUpdated =
                sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At, TestCountryIds.Be]);

            // Assert
            Assert.True(errorsOrUpdated.IsError);

            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Single(competitor.TelevoteAwards));
            Assert.Single(sut.Televotes, televote => televote.PointsAwarded);
        }

        [Fact]
        public void Should_throw_given_null_votingCountryId_arg()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            List<CountryId> arbitraryCompetingCountryIds = [TestCountryIds.At];

            // Act
            Action act = () => sut.AwardTelevotePoints(null!, arbitraryCompetingCountryIds);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'votingCountryId')", exception.Message);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_throw_given_null_rankedCompetingCountryIds_arg()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId arbitraryVotingCountryId = TestCountryIds.At;

            // Act
            Action act = () => sut.AwardTelevotePoints(arbitraryVotingCountryId, null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'rankedCompetingCountryIds')", exception.Message);

            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);
            Assert.All(sut.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
            Assert.All(sut.Televotes, televote => Assert.False(televote.PointsAwarded));
        }
    }

    public sealed class DisqualifyCompetitorMethod : UnitTestBase
    {
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("bf5a7250-3a6b-456f-a3f8-d8280851afff"));
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
        private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;
        private static readonly BroadcastDate BroadcastDate1May2025 =
            BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-01", "yyyy-mm-dd")).Value;

        [Fact]
        public void Should_remove_disqualified_Competitor_and_update_FinishingPosition_values_of_remainder_scenario_1()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId competitor1Id = sut.Competitors[0].CompetingCountryId;

            // Assert
            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.DisqualifyCompetitor(competitor1Id);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });
        }

        [Fact]
        public void Should_remove_disqualified_Competitor_and_update_FinishingPosition_values_of_remainder_scenario_2()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId competitor2Id = sut.Competitors[1].CompetingCountryId;

            // Assert
            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.DisqualifyCompetitor(competitor2Id);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });
        }

        [Fact]
        public void Should_remove_disqualified_Competitor_and_update_FinishingPosition_values_of_remainder_scenario_3()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId competitor3Id = sut.Competitors[2].CompetingCountryId;

            // Assert
            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.DisqualifyCompetitor(competitor3Id);

            // Assert
            Assert.False(errorsOrUpdated.IsError);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                });
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_instance_BroadcastStatus_value_is_InProgress()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardTelevotePoints(TestCountryIds.Cz, [TestCountryIds.At, TestCountryIds.Be]);

            CountryId arbitraryCountryId = sut.Competitors[0].CompetingCountryId;

            // Assert
            Assert.Equal(BroadcastStatus.InProgress, sut.BroadcastStatus);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.DisqualifyCompetitor(arbitraryCountryId);

            (bool isError, Error firstError) = (errorsOrUpdated.IsError, errorsOrUpdated.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Cannot disqualify", firstError.Code);
            Assert.Equal("A competitor may only be disqualified when the broadcast status is Initialized.",
                firstError.Description);
            Assert.Null(firstError.Metadata);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_instance_BroadcastStatus_value_is_Completed()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            sut.AwardTelevotePoints(TestCountryIds.At, [TestCountryIds.Be, TestCountryIds.Cz]);
            sut.AwardTelevotePoints(TestCountryIds.Be, [TestCountryIds.Cz, TestCountryIds.At]);
            sut.AwardTelevotePoints(TestCountryIds.Cz, [TestCountryIds.At, TestCountryIds.Be]);
            sut.AwardTelevotePoints(TestCountryIds.Xx, [TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz]);

            CountryId arbitraryCountryId = sut.Competitors[0].CompetingCountryId;

            // Assert
            Assert.Equal(BroadcastStatus.Completed, sut.BroadcastStatus);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.DisqualifyCompetitor(arbitraryCountryId);

            (bool isError, Error firstError) = (errorsOrUpdated.IsError, errorsOrUpdated.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Cannot disqualify", firstError.Code);
            Assert.Equal("A competitor may only be disqualified when the broadcast status is Initialized.",
                firstError.Description);
            Assert.Null(firstError.Metadata);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });
        }

        [Fact]
        public void Should_return_Errors_and_not_update_when_competingCountryId_arg_matches_no_Competitor()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            CountryId wrongCountryId = CountryId.FromValue(Guid.Parse("eb3368ee-3261-44d4-8258-45eb3b19dd8a"));

            // Assert
            Assert.Equal(BroadcastStatus.Initialized, sut.BroadcastStatus);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });

            // Act
            ErrorOr<Updated> errorsOrUpdated = sut.DisqualifyCompetitor(wrongCountryId);

            (bool isError, Error firstError) = (errorsOrUpdated.IsError, errorsOrUpdated.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Competitor not found", firstError.Code);
            Assert.Equal("Broadcast has no competitor with the provided competing country ID.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == wrongCountryId.Value);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });
        }

        [Fact]
        public void Should_throw_given_null_competingCountryId_arg()
        {
            // Arrange
            Contest contest = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            Broadcast sut = contest.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId)).Value;

            // Assert
            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });

            // Act
            Action act = () => sut.DisqualifyCompetitor(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryId')", exception.Message);

            Assert.Collection(sut.Competitors, ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(1, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.At, competitor.CompetingCountryId);
                    Assert.Equal(1, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(2, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Be, competitor.CompetingCountryId);
                    Assert.Equal(2, competitor.RunningOrderPosition);
                },
                ([UsedImplicitly] competitor) =>
                {
                    Assert.Equal(3, competitor.FinishingPosition);
                    Assert.Equal(TestCountryIds.Cz, competitor.CompetingCountryId);
                    Assert.Equal(3, competitor.RunningOrderPosition);
                });
        }
    }
}
