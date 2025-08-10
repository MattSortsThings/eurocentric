using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.UnitTests.Aggregates.Broadcasts.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;

namespace Eurocentric.Domain.UnitTests.Aggregates.Broadcasts;

public sealed partial class BroadcastTests
{
    [Test]
    public async Task DisqualifyCompetitor_should_remove_Competitor_and_update_remaining_Competitors_scenario_1_of_3()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.DisqualifyCompetitor(AtId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Competitors)
            .HasCount(2)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(2)
                .HasCompetingCountryId(BeId)
                .HasFinishingPosition(1)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(3)
                .HasCompetingCountryId(CzId)
                .HasFinishingPosition(2)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build());

        await Assert.That(sut.Juries)
            .HasCount(3)
            .And.Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .HasCount(3)
            .And.Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());
    }

    [Test]
    public async Task DisqualifyCompetitor_should_remove_Competitor_and_update_remaining_Competitors_scenario_2_of_3()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.DisqualifyCompetitor(BeId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Competitors)
            .HasCount(2)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(1)
                .HasCompetingCountryId(AtId)
                .HasFinishingPosition(1)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(3)
                .HasCompetingCountryId(CzId)
                .HasFinishingPosition(2)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build());

        await Assert.That(sut.Juries)
            .HasCount(3)
            .And.Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .HasCount(3)
            .And.Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());
    }

    [Test]
    public async Task DisqualifyCompetitor_should_remove_Competitor_and_update_remaining_Competitors_scenario_3_of_3()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.DisqualifyCompetitor(CzId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Competitors)
            .HasCount(2)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(1)
                .HasCompetingCountryId(AtId)
                .HasFinishingPosition(1)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(2)
                .HasCompetingCountryId(BeId)
                .HasFinishingPosition(2)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build());

        await Assert.That(sut.Juries)
            .HasCount(3)
            .And.Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .HasCount(3)
            .And.Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());
    }

    [Test]
    public async Task DisqualifyCompetitor_should_fail_and_return_Errors_when_any_Jury_has_true_PointsAwarded()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        sut.AwardJuryPoints(CzId, [AtId, BeId]);

        // Act
        ErrorOr<Updated> result = sut.DisqualifyCompetitor(AtId);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Broadcast running order locked")
            .And.HasDescription("Broadcast running order cannot be modified " +
                                "once at least one jury or televote has awarded its points.");

        await Assert.That(sut.Competitors)
            .HasCount(3)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(1)
                .HasCompetingCountryId(AtId)
                .HasFinishingPosition(1)
                .HasSingleJuryAward(CzId, PointsValue.Twelve)
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(2)
                .HasCompetingCountryId(BeId)
                .HasFinishingPosition(2)
                .HasSingleJuryAward(CzId, PointsValue.Ten)
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(3)
                .HasCompetingCountryId(CzId)
                .HasFinishingPosition(3)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build());

        await Assert.That(sut.Juries)
            .HasCount(3)
            .And.Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(true).Build());

        await Assert.That(sut.Televotes)
            .HasCount(3)
            .And.Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());
    }

    [Test]
    public async Task DisqualifyCompetitor_should_fail_and_return_Errors_when_any_Televote_has_true_PointsAwarded()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        sut.AwardTelevotePoints(CzId, [AtId, BeId]);

        // Act
        ErrorOr<Updated> result = sut.DisqualifyCompetitor(AtId);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Broadcast running order locked")
            .And.HasDescription("Broadcast running order cannot be modified " +
                                "once at least one jury or televote has awarded its points.");

        await Assert.That(sut.Competitors)
            .HasCount(3)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(1)
                .HasCompetingCountryId(AtId)
                .HasFinishingPosition(1)
                .HasNoJuryAwards()
                .HasSingleTelevoteAward(CzId, PointsValue.Twelve).Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(2)
                .HasCompetingCountryId(BeId)
                .HasFinishingPosition(2)
                .HasNoJuryAwards()
                .HasSingleTelevoteAward(CzId, PointsValue.Ten).Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(3)
                .HasCompetingCountryId(CzId)
                .HasFinishingPosition(3)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build());

        await Assert.That(sut.Juries)
            .HasCount(3)
            .And.Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .HasCount(3)
            .And.Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(true).Build());
    }

    [Test]
    public async Task DisqualifyCompetitor_should_fail_and_return_Errors_given_competing_country_ID_matching_no_Competitor()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.DisqualifyCompetitor(FiId);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Disqualified competing country ID mismatch")
            .And.HasDescription("Disqualified competing country ID has no matching competitor in broadcast.");

        await Assert.That(sut.Competitors)
            .HasCount(3)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(1)
                .HasCompetingCountryId(AtId)
                .HasFinishingPosition(1)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(2)
                .HasCompetingCountryId(BeId)
                .HasFinishingPosition(2)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(3)
                .HasCompetingCountryId(CzId)
                .HasFinishingPosition(3)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build());

        await Assert.That(sut.Juries)
            .HasCount(3)
            .And.Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .HasCount(3)
            .And.Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());
    }

    [Test]
    public async Task DisqualifyCompetitor_should_throw_given_null_competingCountryId_arg()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        Action act = () => sut.DisqualifyCompetitor(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'competingCountryId')")
            .WithParameterName("competingCountryId");
    }
}
