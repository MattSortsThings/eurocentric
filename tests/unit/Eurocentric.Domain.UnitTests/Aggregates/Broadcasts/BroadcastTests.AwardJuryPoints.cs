using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Aggregates.Broadcasts.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;

namespace Eurocentric.Domain.UnitTests.Aggregates.Broadcasts;

public sealed partial class BroadcastTests
{
    [Test]
    public async Task AwardJuryPoints_should_set_Completed_to_false_when_any_Jury_or_Televote_remains_false_PointsAwarded()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Juries).IsNotEmpty()
            .And.Contains(Matchers.Jury().HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes).IsNotEmpty()
            .And.Contains(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_not_add_domain_event_when_any_Jury_or_Televote_remains_false_PointsAwarded()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.DetachAllDomainEvents()).IsEmpty();
    }

    [Test]
    public async Task AwardJuryPoints_should_set_Completed_to_true_when_no_Jury_or_Televote_remains_false_PointsAwarded()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        sut.AwardTelevotePoints(AtId, [BeId, CzId]);
        sut.AwardTelevotePoints(BeId, [AtId, CzId]);
        sut.AwardTelevotePoints(CzId, [AtId, BeId]);

        sut.AwardJuryPoints(BeId, [AtId, CzId]);
        sut.AwardJuryPoints(CzId, [AtId, BeId]);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Juries).IsNotEmpty()
            .And.ContainsOnly(Matchers.Jury().HasPointsAwarded(true).Build());

        await Assert.That(sut.Televotes).IsNotEmpty()
            .And.ContainsOnly(Matchers.Televote().HasPointsAwarded(true).Build());

        await Assert.That(sut.Completed).IsTrue();
    }

    [Test]
    public async Task AwardJuryPoints_should_add_domain_event_when_no_Jury_or_Televote_remains_false_PointsAwarded()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        sut.AwardTelevotePoints(AtId, [BeId, CzId]);
        sut.AwardTelevotePoints(BeId, [AtId, CzId]);
        sut.AwardTelevotePoints(CzId, [AtId, BeId]);

        sut.AwardJuryPoints(BeId, [AtId, CzId]);
        sut.AwardJuryPoints(CzId, [AtId, BeId]);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        IDomainEvent? domainEvent = await Assert.That(sut.DetachAllDomainEvents()).HasSingleItem();

        await Assert.That(domainEvent).IsNotNull()
            .And.IsTypeOf<BroadcastCompletedEvent>();
    }

    [Test]
    public async Task AwardJuryPoints_should_updated_ranked_Competitors_FinishingPosition_values_and_JuryAwards()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Competitors)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(1)
                .HasCompetingCountryId(AtId)
                .HasFinishingPosition(3)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(2)
                .HasCompetingCountryId(BeId)
                .HasFinishingPosition(1)
                .HasSingleJuryAward(AtId, PointsValue.Twelve)
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(3)
                .HasCompetingCountryId(CzId)
                .HasFinishingPosition(2)
                .HasSingleJuryAward(AtId, PointsValue.Ten)
                .HasNoTelevoteAwards().Build());
    }

    [Test]
    public async Task AwardJuryPoints_should_award_top_ten_ranked_Competitors_12_points_to_1_point_and_all_others_0_points()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId, DkId, EeId, FiId, GbId],
            group2CountryIds: [HrId, ItId, JpId, KgId, LuId, MkId, XxId]);

        Broadcast sut = CreateGrandFinalBroadcastWithCompetitors(parentContest,
            AtId, BeId, CzId, DkId, EeId, FiId, GbId, HrId, ItId, JpId, KgId, LuId, MkId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(XxId,
            [AtId, BeId, CzId, DkId, EeId, FiId, GbId, HrId, ItId, JpId, KgId, LuId, MkId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Competitors)
            .Contains(Matchers.Competitor()
                .HasCompetingCountryId(AtId).HasSingleJuryAward(XxId, PointsValue.Twelve).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(BeId).HasSingleJuryAward(XxId, PointsValue.Ten).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(CzId).HasSingleJuryAward(XxId, PointsValue.Eight).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(DkId).HasSingleJuryAward(XxId, PointsValue.Seven).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(EeId).HasSingleJuryAward(XxId, PointsValue.Six).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(FiId).HasSingleJuryAward(XxId, PointsValue.Five).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(GbId).HasSingleJuryAward(XxId, PointsValue.Four).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(HrId).HasSingleJuryAward(XxId, PointsValue.Three).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(ItId).HasSingleJuryAward(XxId, PointsValue.Two).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(JpId).HasSingleJuryAward(XxId, PointsValue.One).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(KgId).HasSingleJuryAward(XxId, PointsValue.Zero).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(LuId).HasSingleJuryAward(XxId, PointsValue.Zero).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(MkId).HasSingleJuryAward(XxId, PointsValue.Zero).Build());
    }

    [Test]
    public async Task AwardJuryPoints_should_set_Jury_PointsAwarded_to_true()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Juries)
            .Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(true).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_and_return_Errors_given_voting_country_ID_matching_no_Jury()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(FiId, [BeId, CzId, BeId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Jury voting country ID mismatch")
            .And.HasDescription("Voting country ID must match a jury in the broadcast that has not yet awarded its points.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Juries)
            .ContainsOnly(Matchers.Jury().HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_and_return_Errors_given_voting_country_ID_of_Jury_true_PointsAwarded()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Jury voting country ID mismatch")
            .And.HasDescription("Voting country ID must match a jury in the broadcast that has not yet awarded its points.");

        await Assert.That(sut.Competitors)
            .Contains(Matchers.Competitor()
                .HasCompetingCountryId(BeId)
                .HasSingleJuryAward(AtId, PointsValue.Twelve)
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(CzId)
                .HasSingleJuryAward(AtId, PointsValue.Ten)
                .HasNoTelevoteAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(AtId)
                .HasNoJuryAwards()
                .HasNoTelevoteAwards().Build());

        await Assert.That(sut.Juries)
            .Contains(Matchers.Jury().HasVotingCountryId(AtId).HasPointsAwarded(true).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Jury().HasVotingCountryId(CzId).HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_and_return_Errors_given_duplicate_competing_country_IDs()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId, BeId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Juries)
            .ContainsOnly(Matchers.Jury().HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_and_return_Errors_given_competing_country_ID_matching_no_Competitor()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId, FiId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Juries)
            .ContainsOnly(Matchers.Jury().HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_and_return_Errors_given_competing_country_ID_omitting_Competitor()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Juries)
            .ContainsOnly(Matchers.Jury().HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_fail_and_return_Errors_given_competing_country_ID_equal_to_voting_country_ID()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardJuryPoints(AtId, [BeId, CzId, AtId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Juries)
            .ContainsOnly(Matchers.Jury().HasPointsAwarded(false).Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task AwardJuryPoints_should_throw_given_null_votingCountryId_arg()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId);

        CountryId[] arbitraryCompetingCountryIds = [AtId, BeId];

        Action act = () => sut.AwardJuryPoints(null!, arbitraryCompetingCountryIds);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'votingCountryId')")
            .WithParameterName("votingCountryId");
    }

    [Test]
    public async Task AwardJuryPoints_should_throw_given_null_rankedCompetingCountryIds_arg()
    {
        // Arrange
        StockholmFormatContest parentContest = CreateStockholmFormatContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId);

        CountryId arbitraryVotingCountryId = XxId;

        Action act = () => sut.AwardJuryPoints(arbitraryVotingCountryId, null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'rankedCompetingCountryIds')")
            .WithParameterName("rankedCompetingCountryIds");
    }
}
