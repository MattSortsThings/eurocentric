using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Aggregates.Contests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests;

public sealed partial class StockholmFormatContestTests
{
    [Test]
    [Arguments(2016, "2016-01-01", "bd9c7868-df7a-4492-b5c1-82d7585c8ad4")]
    [Arguments(2050, "2050-12-31", "7f6df94a-6c76-42b1-910c-18d8b1eb3807")]
    public async Task CreateGrandFinal_Build_should_return_Broadcast_with_provided_BroadcastDate_and_Id(int contestYearValue,
        string broadcastDateValue,
        string broadcastIdValue)
    {
        // Arrange
        ContestYear contestYear = ContestYear.FromValue(contestYearValue).Value;
        BroadcastDate broadcastDate = BroadcastDate.FromValue(DateOnly.ParseExact(broadcastDateValue, DateFormat)).Value;
        BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse(broadcastIdValue));

        StockholmFormatContest sut = CreateContest(contestYear: contestYear,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        CountryId[] arbitraryCompetingCountryIds = [AtId, FiId];

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(broadcastDate)
            .WithCompetingCountryIds(arbitraryCompetingCountryIds)
            .Build(() => broadcastId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert.That(broadcast.BroadcastDate).IsEqualTo(broadcastDate);
        await Assert.That(broadcast.Id).IsEqualTo(broadcastId);
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_GrandFinal_Broadcast_with_false_Completed_and_referencing_parent()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([AtId, DkId])
            .Build(() => FixedBroadcastId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert.That(broadcast.ContestStage).IsEqualTo(ContestStage.GrandFinal);
        await Assert.That(broadcast.Completed).IsFalse();
        await Assert.That(broadcast.ParentContestId).IsEqualTo(sut.Id);
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Broadcast_with_Competitors_in_specified_order()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([CzId, DkId, AtId, FiId, BeId])
            .Build(() => FixedBroadcastId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert.That(broadcast.Competitors).HasCount(5)
            .And.Contains(Matchers.InitializedCompetitor(CzId, runningOrderPosition: 1, finishingPosition: 1))
            .And.Contains(Matchers.InitializedCompetitor(DkId, runningOrderPosition: 2, finishingPosition: 2))
            .And.Contains(Matchers.InitializedCompetitor(AtId, runningOrderPosition: 3, finishingPosition: 3))
            .And.Contains(Matchers.InitializedCompetitor(FiId, runningOrderPosition: 4, finishingPosition: 4))
            .And.Contains(Matchers.InitializedCompetitor(BeId, runningOrderPosition: 5, finishingPosition: 5));
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Broadcast_with_Jury_for_every_Participant()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([AtId, DkId])
            .Build(() => FixedBroadcastId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert.That(broadcast.Juries)
            .HasCount(6)
            .And.Contains(Matchers.InitializedJury(AtId))
            .And.Contains(Matchers.InitializedJury(BeId))
            .And.Contains(Matchers.InitializedJury(CzId))
            .And.Contains(Matchers.InitializedJury(DkId))
            .And.Contains(Matchers.InitializedJury(EeId))
            .And.Contains(Matchers.InitializedJury(FiId));
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Broadcast_with_Televote_for_every_Participant()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([AtId, DkId])
            .Build(() => FixedBroadcastId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert.That(broadcast.Televotes)
            .HasCount(6)
            .And.Contains(Matchers.InitializedTelevote(AtId))
            .And.Contains(Matchers.InitializedTelevote(BeId))
            .And.Contains(Matchers.InitializedTelevote(CzId))
            .And.Contains(Matchers.InitializedTelevote(DkId))
            .And.Contains(Matchers.InitializedTelevote(EeId))
            .And.Contains(Matchers.InitializedTelevote(FiId));
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Errors_when_broadcast_date_not_set()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithCompetingCountryIds([AtId, DkId])
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Unexpected)
            .And.HasCode("Broadcast date not set")
            .And.HasDescription("Broadcast builder invoked without setting broadcast date.");
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Errors_when_competitors_not_set()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Unexpected)
            .And.HasCode("Competitors not set")
            .And.HasDescription("Broadcast builder invoked without setting competitors.");
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Errors_when_instance_has_existing_GrandFinal_ChildBroadcast()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("a0de34f2-af28-4e2a-86f1-510e7b557452"));

        sut.AddChildBroadcast(existingBroadcastId, ContestStage.GrandFinal);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([AtId, DkId])
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Child broadcast contest stage conflict")
            .And.HasDescription("The contest already has a child broadcast with the provided contest stage.")
            .And.HasMetadataEntry("contestStage", "GrandFinal");
    }

    [Test]
    [Arguments("2015-12-31")]
    [Arguments("2051-01-01")]
    public async Task CreateGrandFinal_Build_should_return_Errors_given_illegal_broadcast_date_value(string broadcastDateValue)
    {
        // Arrange
        ErrorOr<BroadcastDate> errorsOrBroadcastDate =
            BroadcastDate.FromValue(DateOnly.ParseExact(broadcastDateValue, DateFormat));

        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(errorsOrBroadcastDate)
            .WithCompetingCountryIds([AtId, DkId])
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal broadcast date value")
            .And.HasDescription("Broadcast date value must have a year between 2016 and 2050.")
            .And.HasMetadataEntry("broadcastDate", broadcastDateValue);
    }

    [Test]
    [MatrixDataSource]
    public async Task CreateGrandFinal_Build_Should_return_Errors_given_broadcast_date_outside_contest_year(
        [Matrix(2023, 2025)] int contestYearValue,
        [Matrix("2016-01-01", "2024-12-31", "2026-01-01", "2050-12-31")]
        string broadcastDateValue)
    {
        ContestYear contestYear = ContestYear.FromValue(contestYearValue).Value;
        BroadcastDate broadcastDate = BroadcastDate.FromValue(DateOnly.ParseExact(broadcastDateValue, DateFormat)).Value;

        StockholmFormatContest sut = CreateContest(contestYear: contestYear,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(broadcastDate)
            .WithCompetingCountryIds([AtId, DkId])
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Child broadcast date out of range")
            .And.HasDescription("Child broadcast date must match parent contest year.")
            .And.HasMetadataEntry("broadcastDate", broadcastDateValue);
    }

    [Test]
    public async Task CreateGrandFinal_Build_Should_return_Errors_given_competing_country_ID_matching_no_participant()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([AtId, BeId, GbId])
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Conflict)
            .And.HasCode("Child broadcast competing country IDs mismatch")
            .And.HasDescription("Every competitor in a child broadcast must have a competing country ID " +
                                "matching a participant in the parent contest " +
                                "that is eligible to compete in the broadcast's contest stage.");
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Errors_given_duplicate_competing_country_IDs()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([AtId, DkId, AtId])
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Duplicate competing country IDs")
            .And.HasDescription("Every competitor in a broadcast must have a different competing country ID.");
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_return_Errors_given_fewer_than_2_competing_country_IDs()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(contestYear: ContestYear2025,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Func<BroadcastId> dummyIdProvider = () => null!;

        // Act
        ErrorOr<Broadcast> result = sut.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds([AtId])
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal competitor count")
            .And.HasDescription("A broadcast must have at least 2 competitors.");
    }

    [Test]
    public async Task CreateGrandFinal_WithCompetingCountryIds_should_throw_given_null_competingCountryIds_arg()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Action act = () => sut.CreateGrandFinal().WithCompetingCountryIds(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'competingCountryIds')")
            .WithParameterName("competingCountryIds");
    }

    [Test]
    public async Task CreateGrandFinal_Build_should_throw_given_null_idProvider_arg()
    {
        // Arrange
        StockholmFormatContest sut = CreateContest(
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Action act = () => sut.CreateGrandFinal().Build(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'idProvider')")
            .WithParameterName("idProvider");
    }
}
