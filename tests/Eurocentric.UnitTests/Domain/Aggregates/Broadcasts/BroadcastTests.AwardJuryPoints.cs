using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.Broadcasts.TestUtils;
using Eurocentric.UnitTests.Domain.Aggregates.TestUtils;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Broadcasts;

public sealed partial class BroadcastTests
{
    [Test]
    public async Task AwardJuryPoints_should_set_Jury_PointsAwarded_to_true()
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
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Juries)
            .Contains(Matchers.Jury().HasVotingCountryId(CountryIds.At).PointsAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Be).PointsNotAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Cz).PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
    }

    [Test]
    public async Task AwardJuryPoints_should_set_Completed_to_false_if_any_Jury_or_Televote_is_not_PointsAwarded()
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
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(sut.Juries).Contains(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).Contains(Matchers.Televote().PointsNotAwarded().Match);

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_set_Completed_to_true_if_all_Juries_and_Televotes_PointsAwarded()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: []
        );

        AwardTelevotePoints(
            sut,
            AwardParams.From(votingCountryId: CountryIds.At, rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]),
            AwardParams.From(votingCountryId: CountryIds.Be, rankedCompetingCountryIds: [CountryIds.Cz, CountryIds.At]),
            AwardParams.From(votingCountryId: CountryIds.Cz, rankedCompetingCountryIds: [CountryIds.At, CountryIds.Be])
        );

        AwardJuryPoints(
            sut,
            AwardParams.From(votingCountryId: CountryIds.At, rankedCompetingCountryIds: [CountryIds.Be, CountryIds.Cz]),
            AwardParams.From(votingCountryId: CountryIds.Be, rankedCompetingCountryIds: [CountryIds.Cz, CountryIds.At])
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Cz,
            rankedCompetingCountryIds: [CountryIds.Be, CountryIds.At]
        );

        // Assert
        await Assert.That(sut.Juries).Contains(Matchers.Jury().PointsNotAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsAwarded().Match);

        await Assert.That(sut.Completed).IsFalse();

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(sut.Juries).ContainsOnly(Matchers.Jury().PointsAwarded().Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsAwarded().Match);

        await Assert.That(sut.Completed).IsTrue();
    }

    [Test]
    public async Task AwardJuryPoints_should_award_top_10_competitors_12_to_1_points_all_others_0_points()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
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
            extraVotingCountryIds: [CountryIds.Pt]
        );

        AwardParams awardParams = AwardParams.From(
            votingCountryId: CountryIds.Pt,
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
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.At)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Twelve)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Ten)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Eight)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Dk)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Seven)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Ee)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Six)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Fi)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Five)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Gb)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Four)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Hr)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Three)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.It)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Two)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Lv)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.One)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.Mt)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Zero)
                    .Match
            )
            .Contains(
                Matchers
                    .Competitor()
                    .HasCompetingCountryId(CountryIds.No)
                    .HasSingleJuryAward(CountryIds.Pt, PointsValue.Zero)
                    .Match
            );
    }

    [Test]
    public async Task AwardJuryPoints_should_update_Competitors_scenario_1()
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
                    .HasNoTelevoteAwards()
                    .HasNoJuryAwards()
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasNoJuryAwards()
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasNoJuryAwards()
                    .HasFinishingPosition(3)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoTelevoteAwards()
                    .HasNoJuryAwards()
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.At, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            );
    }

    [Test]
    public async Task AwardJuryPoints_should_update_Competitors_scenario_2()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardJuryPoints(
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
                    .HasNoTelevoteAwards()
                    .HasNoJuryAwards()
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.At, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.Be, PointsValue.Ten)
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            );
    }

    [Test]
    public async Task AwardJuryPoints_should_update_Competitors_scenario_3()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardJuryPoints(
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
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.Be, PointsValue.Ten)
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasSingleJuryAward(CountryIds.At, PointsValue.Twelve)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.Be, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Cz, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Twelve)
                    .HasJuryAward(CountryIds.Cz, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(3)
                    .Match
            );
    }

    [Test]
    public async Task AwardJuryPoints_should_update_Competitors_scenario_4()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardJuryPoints(
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
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.Be, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Cz, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Twelve)
                    .HasJuryAward(CountryIds.Cz, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Be, PointsValue.Twelve)
                    .HasFinishingPosition(3)
                    .Match
            );

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(sut.Competitors)
            .Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(1)
                    .HasCompetingCountryId(CountryIds.At)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.Be, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Cz, PointsValue.Twelve)
                    .HasJuryAward(CountryIds.Dk, PointsValue.Ten)
                    .HasFinishingPosition(2)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(2)
                    .HasCompetingCountryId(CountryIds.Be)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Twelve)
                    .HasJuryAward(CountryIds.Cz, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Dk, PointsValue.Eight)
                    .HasFinishingPosition(3)
                    .Match
            )
            .And.Contains(
                Matchers
                    .Competitor()
                    .HasRunningOrderSpot(3)
                    .HasCompetingCountryId(CountryIds.Cz)
                    .HasNoTelevoteAwards()
                    .HasJuryAward(CountryIds.At, PointsValue.Ten)
                    .HasJuryAward(CountryIds.Be, PointsValue.Twelve)
                    .HasJuryAward(CountryIds.Dk, PointsValue.Twelve)
                    .HasFinishingPosition(1)
                    .Match
            );
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_jury_voting_country_conflict_scenario_1()
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
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await AssertNoPointsAwarded(sut);

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Jury voting country conflict")
            .And.HasDetail("The requested broadcast has no jury that may award points and has the provided country ID.")
            .And.HasExtension("broadcastId", sut.Id.Value)
            .And.HasExtension("countryId", orphanCountryId.Value);
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_jury_voting_country_conflict_scenario_2()
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

        AwardJuryPoints(sut, awardParams);

        // Assert
        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasSingleJuryAward(CountryIds.Dk).Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
        await Assert
            .That(sut.Juries)
            .Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Dk).PointsAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.At).PointsNotAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Be).PointsNotAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Cz).PointsNotAwarded().Match);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Jury voting country conflict")
            .And.HasDetail("The requested broadcast has no jury that may award points and has the provided country ID.")
            .And.HasExtension("broadcastId", sut.Id.Value)
            .And.HasExtension("countryId", CountryIds.Dk.Value);

        await Assert
            .That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasSingleJuryAward(CountryIds.Dk).Match);
        await Assert.That(sut.Televotes).ContainsOnly(Matchers.Televote().PointsNotAwarded().Match);
        await Assert
            .That(sut.Juries)
            .Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Dk).PointsAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.At).PointsNotAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Be).PointsNotAwarded().Match)
            .And.Contains(Matchers.Jury().HasVotingCountryId(CountryIds.Cz).PointsNotAwarded().Match);
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_ranked_competing_countries_conflict_scenario_1()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        AwardParams awardParams = AwardParams.From(votingCountryId: CountryIds.Dk, rankedCompetingCountryIds: []);

        // Assert
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await AssertNoPointsAwarded(sut);

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_ranked_competing_countries_conflict_scenario_2()
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
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await AssertNoPointsAwarded(sut);

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_ranked_competing_countries_conflict_scenario_3()
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
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

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
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_ranked_competing_countries_conflict_scenario_4()
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
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await AssertNoPointsAwarded(sut);

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_ranked_competing_countries_conflict_scenario_5()
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
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await AssertNoPointsAwarded(sut);

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_ranked_competing_countries_conflict_scenario_6()
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
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await AssertNoPointsAwarded(sut);

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_on_ranked_competing_countries_conflict_scenario_7()
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
        await AssertNoPointsAwarded(sut);

        // Act
        UnitResult<IDomainError> result = sut.AwardJuryPoints(awardParams);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await AssertNoPointsAwarded(sut);

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Ranked competing countries conflict")
            .And.HasDetail(
                "Ranked competing countries must comprise every competing country in the broadcast, "
                    + "excluding the voting country, without duplication."
            )
            .And.HasExtension("broadcastId", sut.Id.Value);
    }

    [Test]
    public async Task AwardJuryPoints_should_throw_given_null_awardParams_arg()
    {
        // Arrange
        Broadcast sut = CreateJuryAndTelevoteBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Dk]
        );

        IAwardParams nullAwardParams = null!;

        // Assert
        await Assert
            .That(() => sut.AwardJuryPoints(nullAwardParams))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'awardParams')");
    }
}
