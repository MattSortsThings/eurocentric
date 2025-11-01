using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.Broadcasts.TestUtils;
using Eurocentric.UnitTests.Domain.Aggregates.TestUtils;
using Eurocentric.UnitTests.TestUtils;
using Broadcast = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;

namespace Eurocentric.UnitTests.Domain.Aggregates.Broadcasts;

public sealed partial class BroadcastTests
{
    [Test]
    public async Task AwardTelevotePoints_should_set_Televote_PointsAwarded_to_true()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: []
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.At,
            rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]
        );

        // Assert
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert
            .That(sut.Televotes)
            .Contains(Matchers.Televote().HasVotingCountryId(CountryIds.At).PointsAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Be).PointsNotAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Cz).PointsNotAwarded().Match);
    }

    [Test]
    public async Task AwardTelevotePoints_should_set_Completed_to_false_if_any_Jury_or_Televote_is_not_PointsAwarded()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: []
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.At,
            rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]
        );

        // Assert
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        await Assert.That(sut.Completed).IsFalse();

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(sut.Juries).Contains(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).Contains(Matchers.Televote().PointsNotAwarded().Match);

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardTelevotePoints_should_set_Completed_to_true_if_all_Juries_and_Televotes_PointsAwarded()
    {
        // Arrange
        Broadcast sut = CreateTelevoteOnlyBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Xx]
        );

        AwardTelevotePoints(
            sut,
            AwardParams.From(votingCountryId: CountryIds.At, rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]),
            AwardParams.From(votingCountryId: CountryIds.Be, rankedCompetingCountryIds: [CountryIds.Cz, CountryIds.At]),
            AwardParams.From(votingCountryId: CountryIds.Cz, rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be])
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Xx,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz]
        );

        // Assert
        await Assert.That(sut.Juries).IsEmpty();
        await Assert.That(sut.Televotes).Contains(Matchers.Televote().PointsNotAwarded().Match);

        await Assert.That(sut.Completed).IsFalse();

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(sut.Juries).IsEmpty();
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsAwarded().Match);

        await Assert.That(sut.Completed).IsTrue();
    }

    [Test]
    public async Task AwardTelevotePoints_should_award_top_10_competitors_12_to_1_points_all_others_0_points()
    {
        // Arrange
        Broadcast sut = CreateTelevoteOnlyBroadcast(
            competingCountryIds:
            [
                CountryIds.At,
                CountryIds.Be,
                CountryIds.Cz,
                CountryIds.Dk,
                CountryIds.Ee,
                CountryIds.Fi,
                CountryIds.Gb,
                CountryIds.Hr,
                CountryIds.It,
                CountryIds.Lv,
                CountryIds.Mt,
                CountryIds.No,
            ],
            extraVotingCountryIds: [CountryIds.Xx]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Xx,
            rankedCompetingCountryIds:
            [
                CountryIds.At,
                CountryIds.Be,
                CountryIds.Cz,
                CountryIds.Dk,
                CountryIds.Ee,
                CountryIds.Fi,
                CountryIds.Gb,
                CountryIds.Hr,
                CountryIds.It,
                CountryIds.Lv,
                CountryIds.Mt,
                CountryIds.No,
            ]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.At)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Twelve)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Ten)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Eight)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Dk)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Seven)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Ee)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Six)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Fi)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Five)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Gb)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Four)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Hr)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Three)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.It)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Two)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Lv)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.One)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Mt)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Zero)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.No)
                    .HasSingleTelevoteAward(CountryIds.Xx, PointsValue.Zero)
                    .Match
            );
    }

    [Test]
    public async Task AwardTelevotePoints_should_update_Competitors_scenario_1()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.At,
            rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasNoTelevoteAwards()
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasNoTelevoteAwards()
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasNoTelevoteAwards()
                    .HasFinishingPosition(3)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasNoTelevoteAwards()
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.At, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            );
    }

    [Test]
    public async Task AwardTelevotePoints_should_update_Competitors_scenario_2()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardTelevotePoints(
            sut,
            AwardParams.From(votingCountryId: CountryIds.At, rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz])
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Be,
            rankedCompetingCountryIds: [CountryIds.Cz, CountryIds.At]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasNoTelevoteAwards()
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.At, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.Be, PointsValue.Ten)
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            );
    }

    [Test]
    public async Task AwardTelevotePoints_should_update_Competitors_scenario_3()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardTelevotePoints(
            sut,
            AwardParams.From(votingCountryId: CountryIds.At, rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]),
            AwardParams.From(votingCountryId: CountryIds.Be, rankedCompetingCountryIds: [CountryIds.Cz, CountryIds.At])
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Cz,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.Be, PointsValue.Ten)
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasSingleTelevoteAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Cz, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Twelve)
                    .HasTelevoteAward(CountryIds.Cz, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(3)
                    .Match
            );
    }

    [Test]
    public async Task AwardTelevotePoints_should_update_Competitors_scenario_4()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardTelevotePoints(
            sut,
            AwardParams.From(votingCountryId: CountryIds.At, rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]),
            AwardParams.From(votingCountryId: CountryIds.Be, rankedCompetingCountryIds: [CountryIds.Cz, CountryIds.At]),
            AwardParams.From(votingCountryId: CountryIds.Cz, rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be])
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Dk,
            rankedCompetingCountryIds: [CountryIds.Cz, CountryIds.At, CountryIds.Be]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Cz, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Twelve)
                    .HasTelevoteAward(CountryIds.Cz, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(3)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Cz, PointsValue.Twelve)
                    .HasTelevoteAward(CountryIds.Dk, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Twelve)
                    .HasTelevoteAward(CountryIds.Cz, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Dk, PointsValue.Eight)
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoJuryAwards()
                    .HasTelevoteAward(CountryIds.At, PointsValue.Ten)
                    .HasTelevoteAward(CountryIds.Be, PointsValue.Twelve)
                    .HasTelevoteAward(CountryIds.Dk, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            );
    }

    [Test]
    public async Task Should_fail_on_televote_voting_country_conflict_scenario_1()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        CountryId orphanCountryId = CountryIds.Generate(1).Single();

        AwardParams awardParams = AwardParams.From(
            votingCountryId: orphanCountryId,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Televote voting country conflict")
            .And.HasDetail(
                "The requested broadcast has no televote that may award points and has the provided country ID."
            )
            .And.HasExtension("broadcastId", sut.Id.Value)
            .And.HasExtension("countryId", orphanCountryId.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_televote_voting_country_conflict_scenario_2()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Dk,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz]
        );

        AwardTelevotePoints(sut, awardParams);

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasSingleTelevoteAward(CountryIds.Dk).Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert
            .That(sut.Televotes)
            .Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Dk).PointsAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.At).PointsNotAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Be).PointsNotAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Cz).PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Televote voting country conflict")
            .And.HasDetail(
                "The requested broadcast has no televote that may award points and has the provided country ID."
            )
            .And.HasExtension("broadcastId", sut.Id.Value)
            .And.HasExtension("countryId", CountryIds.Dk.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasSingleTelevoteAward(CountryIds.Dk).Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert
            .That(sut.Televotes)
            .Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Dk).PointsAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.At).PointsNotAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Be).PointsNotAwarded().Match)
            .And.Contains(Matchers.Televote().HasVotingCountryId(CountryIds.Cz).PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_1()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(votingCountryId: CountryIds.Dk, rankedCompetingCountryIds: []);

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_2()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Dk,
            rankedCompetingCountryIds: [CountryIds.At]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_3()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Dk,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_4()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Dk,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz, CountryIds.At]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_5()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        CountryId orphanCountryId = CountryIds.Generate(1).Single();

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Dk,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz, orphanCountryId]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_6()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Dk,
            rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz, CountryIds.At]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task Should_fail_on_ranked_competing_countries_conflict_scenario_7()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.At,
            rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz, CountryIds.At]
        );

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardTelevotePoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoJuryAwards().HasNoTelevoteAwards().Match);
        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task AwardTelevotePoints_should_fail_given_null_awardParams_arg()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        IAwardParams nullAwardParams = null!;

        // Assert
        await Assert
            .That(() => sut.AwardTelevotePoints(nullAwardParams))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'awardParams')");
    }
}
