using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Aggregates.Broadcasts.Utils;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Broadcasts;

public sealed class BroadcastTests : UnitTest
{
    private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;

    private static readonly BroadcastDate BroadcastDate2025May1 =
        BroadcastDate.FromValue(new DateOnly(2025, 5, 1)).Value;

    private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
    private static readonly ActName ArbitraryActName = ActName.FromValue("ActName").Value;
    private static readonly SongTitle ArbitrarySongTitle = SongTitle.FromValue("SongTitle").Value;

    private static readonly ContestId FixedContestId = ContestId.FromValue(Guid.Parse("b3ee5f9f-7bf3-4bcf-9a12-c594b105a77f"));

    private static readonly BroadcastId FixedBroadcastId =
        BroadcastId.FromValue(Guid.Parse("eb3036eb-a2b3-450f-b57b-2bfd4516b523"));

    private static readonly CountryId AtId = CountryId.FromValue(Guid.Parse("01abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId BeId = CountryId.FromValue(Guid.Parse("02abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId CzId = CountryId.FromValue(Guid.Parse("03abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId DkId = CountryId.FromValue(Guid.Parse("04abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId EeId = CountryId.FromValue(Guid.Parse("05abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId FiId = CountryId.FromValue(Guid.Parse("06abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId GbId = CountryId.FromValue(Guid.Parse("07abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId HrId = CountryId.FromValue(Guid.Parse("08abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId ItId = CountryId.FromValue(Guid.Parse("09abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId JpId = CountryId.FromValue(Guid.Parse("10abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId KgId = CountryId.FromValue(Guid.Parse("11abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId LuId = CountryId.FromValue(Guid.Parse("12abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId MkId = CountryId.FromValue(Guid.Parse("13abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId XxId = CountryId.FromValue(Guid.Parse("24abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));

    private static LiverpoolFormatContest CreateLiverpoolFormatContest(CountryId[] group2CountryIds = null!,
        CountryId[] group1CountryIds = null!,
        CountryId group0CountryId = null!)
    {
        ContestBuilder builder = LiverpoolFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup0Participant(group0CountryId);

        foreach (CountryId countryId in group1CountryIds)
        {
            builder.AddGroup1Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        foreach (CountryId countryId in group2CountryIds)
        {
            builder.AddGroup2Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        ErrorOr<Contest> result = builder.Build(() => FixedContestId);

        return result.Value as LiverpoolFormatContest ?? throw new InvalidCastException("Contest not created");
    }

    private static Broadcast CreateSemiFinal1BroadcastWithCompetitors(Contest parentContest,
        params CountryId[] competingCountryIds)
    {
        ErrorOr<Broadcast> result = parentContest.CreateSemiFinal1()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds(competingCountryIds)
            .Build(() => FixedBroadcastId);

        return result.Value;
    }

    private static Broadcast CreateGrandFinalBroadcastWithCompetitors(Contest parentContest,
        params CountryId[] competingCountryIds)
    {
        ErrorOr<Broadcast> result = parentContest.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds(competingCountryIds)
            .Build(() => FixedBroadcastId);

        return result.Value;
    }

    [Test]
    public async Task Should_set_Completed_to_false_when_any_Jury_or_Televote_has_false_PointsAwarded_after_points_awarded()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Juries).IsEmpty();
        await Assert.That(sut.Televotes).IsNotEmpty()
            .And.Contains(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task Should_set_Completed_to_true_when_no_Jury_or_Televote_has_false_PointsAwarded_after_points_awarded()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        sut.AwardTelevotePoints(BeId, [AtId, CzId]);
        sut.AwardTelevotePoints(CzId, [AtId, BeId]);
        sut.AwardTelevotePoints(XxId, [AtId, BeId, CzId]);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Juries).IsEmpty();
        await Assert.That(sut.Televotes).IsNotEmpty()
            .And.ContainsOnly(Matchers.Televote().HasPointsAwarded(true).Build());

        await Assert.That(sut.Completed).IsTrue();
    }

    [Test]
    public async Task Should_update_ranked_Competitors_FinishingPosition_values_and_TelevoteAwards_collections()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Competitors)
            .Contains(Matchers.Competitor()
                .HasRunningOrderPosition(1)
                .HasCompetingCountryId(AtId)
                .HasFinishingPosition(3)
                .HasNoTelevoteAwards()
                .HasNoJuryAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(2)
                .HasCompetingCountryId(BeId)
                .HasFinishingPosition(1)
                .HasSingleTelevoteAward(AtId, PointsValue.Twelve)
                .HasNoJuryAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasRunningOrderPosition(3)
                .HasCompetingCountryId(CzId)
                .HasFinishingPosition(2)
                .HasSingleTelevoteAward(AtId, PointsValue.Ten)
                .HasNoJuryAwards().Build());
    }

    [Test]
    public async Task Should_award_top_ten_ranked_Competitors_12_points_to_1_point_and_all_others_0_points()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId, DkId, EeId, FiId, GbId],
            group2CountryIds: [HrId, ItId, JpId, KgId, LuId, MkId]);

        Broadcast sut = CreateGrandFinalBroadcastWithCompetitors(parentContest,
            AtId, BeId, CzId, DkId, EeId, FiId, GbId, HrId, ItId, JpId, KgId, LuId, MkId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(XxId,
            [AtId, BeId, CzId, DkId, EeId, FiId, GbId, HrId, ItId, JpId, KgId, LuId, MkId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Competitors)
            .Contains(Matchers.Competitor()
                .HasCompetingCountryId(AtId).HasSingleTelevoteAward(XxId, PointsValue.Twelve).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(BeId).HasSingleTelevoteAward(XxId, PointsValue.Ten).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(CzId).HasSingleTelevoteAward(XxId, PointsValue.Eight).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(DkId).HasSingleTelevoteAward(XxId, PointsValue.Seven).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(EeId).HasSingleTelevoteAward(XxId, PointsValue.Six).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(FiId).HasSingleTelevoteAward(XxId, PointsValue.Five).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(GbId).HasSingleTelevoteAward(XxId, PointsValue.Four).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(HrId).HasSingleTelevoteAward(XxId, PointsValue.Three).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(ItId).HasSingleTelevoteAward(XxId, PointsValue.Two).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(JpId).HasSingleTelevoteAward(XxId, PointsValue.One).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(KgId).HasSingleTelevoteAward(XxId, PointsValue.Zero).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(LuId).HasSingleTelevoteAward(XxId, PointsValue.Zero).Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(MkId).HasSingleTelevoteAward(XxId, PointsValue.Zero).Build());
    }

    [Test]
    public async Task Should_set_Televote_PointsAwarded_to_true()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(sut.Televotes)
            .Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(true).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(XxId).HasPointsAwarded(false).Build());
    }

    [Test]
    public async Task Should_return_Errors_and_not_update_given_voting_country_ID_matching_no_Televote()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(FiId, [BeId, CzId, BeId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Televote voting country ID mismatch")
            .And.HasDescription("Voting country ID must match a televote in the broadcast that has not yet awarded its points.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task Should_return_Errors_and_not_update_given_voting_country_ID_matching_Televote_with_true_PointsAwarded()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        sut.AwardTelevotePoints(AtId, [BeId, CzId]);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Televote voting country ID mismatch")
            .And.HasDescription("Voting country ID must match a televote in the broadcast that has not yet awarded its points.");

        await Assert.That(sut.Competitors)
            .Contains(Matchers.Competitor()
                .HasCompetingCountryId(BeId)
                .HasSingleTelevoteAward(AtId, PointsValue.Twelve)
                .HasNoJuryAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(CzId)
                .HasSingleTelevoteAward(AtId, PointsValue.Ten)
                .HasNoJuryAwards().Build())
            .And.Contains(Matchers.Competitor()
                .HasCompetingCountryId(AtId)
                .HasNoTelevoteAwards()
                .HasNoJuryAwards().Build());

        await Assert.That(sut.Televotes)
            .Contains(Matchers.Televote().HasVotingCountryId(AtId).HasPointsAwarded(true).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(BeId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(CzId).HasPointsAwarded(false).Build())
            .And.Contains(Matchers.Televote().HasVotingCountryId(XxId).HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task Should_return_Errors_and_not_update_given_duplicate_competing_country_IDs()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId, BeId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task Should_return_Errors_and_not_update_given_competing_country_ID_matching_no_Competitor()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId, FiId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task Should_return_Errors_and_not_update_given_competing_country_ID_omitting_Competitor()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task Should_return_Errors_and_not_update_given_competing_country_ID_equal_to_voting_country_ID()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId, CzId);

        // Act
        ErrorOr<Updated> result = sut.AwardTelevotePoints(AtId, [BeId, CzId, AtId]);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Ranked competing country IDs mismatch")
            .And.HasDescription("Ranked competing country IDs must comprise every competing country ID in the broadcast, " +
                                "excluding the voting country ID, exactly once.");

        await Assert.That(sut.Competitors)
            .ContainsOnly(Matchers.Competitor().HasNoTelevoteAwards().HasNoJuryAwards().Build());

        await Assert.That(sut.Televotes)
            .ContainsOnly(Matchers.Televote().HasPointsAwarded(false).Build());

        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task Should_throw_given_null_votingCountryId_arg()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId);

        CountryId[] arbitraryCompetingCountryIds = [AtId, BeId];

        Action act = () => sut.AwardTelevotePoints(null!, arbitraryCompetingCountryIds);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'votingCountryId')")
            .WithParameterName("votingCountryId");
    }

    [Test]
    public async Task Should_throw_given_null_rankedCompetingCountryIds_arg()
    {
        // Arrange
        LiverpoolFormatContest parentContest = CreateLiverpoolFormatContest(
            group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Broadcast sut = CreateSemiFinal1BroadcastWithCompetitors(parentContest, AtId, BeId);

        CountryId arbitraryVotingCountryId = XxId;

        Action act = () => sut.AwardTelevotePoints(arbitraryVotingCountryId, null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'rankedCompetingCountryIds')")
            .WithParameterName("rankedCompetingCountryIds");
    }
}
