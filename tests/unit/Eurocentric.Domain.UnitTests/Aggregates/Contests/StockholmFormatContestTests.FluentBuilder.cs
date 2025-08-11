using ErrorOr;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Aggregates.Contests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests;

public sealed partial class StockholmFormatContestTests
{
    [Test]
    [Arguments(2016, "Stockholm", "bd9c7868-df7a-4492-b5c1-82d7585c8ad4")]
    [Arguments(2050, "San Marino", "7f6df94a-6c76-42b1-910c-18d8b1eb3807")]
    public async Task FluentBuilder_should_return_Contest_with_provided_ContestYear_and_CityName_and_ID(int contestYear,
        string cityName,
        string contestIdValue)
    {
        // Arrange
        ErrorOr<ContestYear> errorsOrContestYear = ContestYear.FromValue(contestYear);
        ErrorOr<CityName> errorsOrCityName = CityName.FromValue(cityName);
        Guid contestId = Guid.Parse(contestIdValue);

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(errorsOrContestYear)
            .WithCityName(errorsOrCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(() => ContestId.FromValue(contestId));

        // Assert
        await Assert.That(result.IsError).IsFalse();

        StockholmFormatContest contest = await Assert.That(result.Value).IsTypeOf<StockholmFormatContest>()
            .And.IsNotNull();

        await Assert.That(contest.Id).HasMember(id => id.Value).EqualTo(contestId);
        await Assert.That(contest.ContestYear).HasMember(year => year.Value).EqualTo(contestYear);
        await Assert.That(contest.CityName).HasMember(name => name.Value).EqualTo(cityName);
        await Assert.That(contest.ContestFormat).IsEqualTo(ContestFormat.Stockholm);
    }

    [Test]
    public async Task FluentBuilder_should_return_Contest_with_false_Completed_and_empty_ChildBroadcasts()
    {
        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(() => FixedContestId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        StockholmFormatContest contest = await Assert.That(result.Value).IsTypeOf<StockholmFormatContest>()
            .And.IsNotNull();

        await Assert.That(contest.Completed).IsFalse();
        await Assert.That(contest.ChildBroadcasts).IsEmpty();
    }

    [Test]
    public async Task FluentBuilder_should_return_Contest_with_specified_Participants()
    {
        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ActName.FromValue("AT act"), SongTitle.FromValue("AT song"))
            .AddGroup1Participant(CzId, ActName.FromValue("CZ act"), SongTitle.FromValue("CZ song"))
            .AddGroup1Participant(FiId, ActName.FromValue("FI act"), SongTitle.FromValue("FI song"))
            .AddGroup2Participant(BeId, ActName.FromValue("BE act"), SongTitle.FromValue("BE song"))
            .AddGroup2Participant(DkId, ActName.FromValue("DK act"), SongTitle.FromValue("DK song"))
            .AddGroup2Participant(EeId, ActName.FromValue("EE act"), SongTitle.FromValue("EE song"))
            .Build(() => FixedContestId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        StockholmFormatContest contest = await Assert.That(result.Value).IsTypeOf<StockholmFormatContest>()
            .And.IsNotNull();

        await Assert.That(contest.Participants)
            .HasCount(6)
            .And.Contains(Matchers.Group1Participant(AtId, actName: "AT act", songTitle: "AT song"))
            .And.Contains(Matchers.Group1Participant(CzId, actName: "CZ act", songTitle: "CZ song"))
            .And.Contains(Matchers.Group1Participant(FiId, actName: "FI act", songTitle: "FI song"))
            .And.Contains(Matchers.Group2Participant(BeId, actName: "BE act", songTitle: "BE song"))
            .And.Contains(Matchers.Group2Participant(DkId, actName: "DK act", songTitle: "DK song"))
            .And.Contains(Matchers.Group2Participant(EeId, actName: "EE act", songTitle: "EE song"));
    }

    [Test]
    public async Task FluentBuilder_should_return_Contest_with_added_domain_event()
    {
        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(() => FixedContestId);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        StockholmFormatContest contest = await Assert.That(result.Value).IsTypeOf<StockholmFormatContest>()
            .And.IsNotNull();

        IDomainEvent? singleDomainEvent = await Assert.That(contest.DetachAllDomainEvents()).HasSingleItem();

        await Assert.That(singleDomainEvent).IsNotNull()
            .And.IsTypeOf<ContestCreatedEvent>();
    }

    [Test]
    public async Task FluentBuilder_should_return_Errors_when_ContestYear_not_provided()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Unexpected)
            .And.HasCode("Contest year not set")
            .And.HasDescription("Contest builder invoked without setting contest year.");
    }

    [Test]
    public async Task FluentBuilder_should_return_Errors_when_CityName_not_provided()
    {
        // Arrange
        const string cityNameValue = "";
        ErrorOr<CityName> illegalCityName = CityName.FromValue(cityNameValue);

        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(illegalCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal city name value")
            .And.HasDescription("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("cityName", cityNameValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_illegal_contest_year_value()
    {
        // Arrange
        const int contestYearValue = 2051;
        ErrorOr<ContestYear> illegalContestYear = ContestYear.FromValue(contestYearValue);

        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(illegalContestYear)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal contest year value")
            .And.HasDescription("Contest year value must be an integer between 2016 and 2050.")
            .And.HasMetadataEntry("contestYear", contestYearValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_illegal_city_name_value()
    {
        // Arrange
        const string cityNameValue = "";
        ErrorOr<CityName> illegalCityName = CityName.FromValue(cityNameValue);

        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(illegalCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal city name value")
            .And.HasDescription("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("cityName", cityNameValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_illegal_group_1_participant_act_name_value()
    {
        // Arrange
        const string actNameValue = "";
        ErrorOr<ActName> illegalActName = ActName.FromValue(actNameValue);

        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, illegalActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal act name value")
            .And.HasDescription("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("actName", actNameValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_illegal_group_2_participant_act_name_value()
    {
        // Arrange
        const string actNameValue = "";
        ErrorOr<ActName> illegalActName = ActName.FromValue(actNameValue);

        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup0Participant(XxId)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, illegalActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal act name value")
            .And.HasDescription("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("actName", actNameValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_illegal_group_1_participant_song_title_value()
    {
        // Arrange
        const string songTitleValue = "";
        ErrorOr<SongTitle> illegalSongTitle = SongTitle.FromValue(songTitleValue);

        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, illegalSongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal song title value")
            .And.HasDescription("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("songTitle", songTitleValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_illegal_group_2_participant_song_title_value()
    {
        // Arrange
        const string songTitleValue = "";
        ErrorOr<SongTitle> illegalSongTitle = SongTitle.FromValue(songTitleValue);

        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, illegalSongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal song title value")
            .And.HasDescription("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("songTitle", songTitleValue);
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_duplicate_group_1_and_2_participating_country_IDs()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup0Participant(XxId)
            .AddGroup1Participant(GbId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(GbId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Duplicate participating country IDs")
            .And.HasDescription("Every participant in a contest must have a different participating country ID.");
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_duplicate_group_1_participating_country_IDs()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(GbId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(GbId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Duplicate participating country IDs")
            .And.HasDescription("Every participant in a contest must have a different participating country ID.");
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_duplicate_group_2_participating_country_IDs()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(GbId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(GbId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Duplicate participating country IDs")
            .And.HasDescription("Every participant in a contest must have a different participating country ID.");
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_one_group_0_participant()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup0Participant(XxId)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal Stockholm format participant groups")
            .And.HasDescription("A Stockholm format contest must have no group 0 participants, " +
                                "at least three group 1 participants, and at least three group 2 participants.");
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_multiple_0_participants()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup0Participant(XxId)
            .AddGroup0Participant(GbId)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal Stockholm format participant groups")
            .And.HasDescription("A Stockholm format contest must have no group 0 participants, " +
                                "at least three group 1 participants, and at least three group 2 participants.");
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_fewer_than_three_group_1_participants()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(FiId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal Stockholm format participant groups")
            .And.HasDescription("A Stockholm format contest must have no group 0 participants, " +
                                "at least three group 1 participants, and at least three group 2 participants.");
    }

    [Test]
    public async Task FluentBuilder_should_return_errors_given_fewer_than_three_group_2_participants()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup1Participant(AtId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(BeId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup1Participant(CzId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(DkId, ArbitraryActName, ArbitrarySongTitle)
            .AddGroup2Participant(EeId, ArbitraryActName, ArbitrarySongTitle)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal Stockholm format participant groups")
            .And.HasDescription("A Stockholm format contest must have no group 0 participants, " +
                                "at least three group 1 participants, and at least three group 2 participants.");
    }

    [Test]
    public async Task FluentBuilder_should_return_Errors_when_no_Participants_added()
    {
        // Arrange
        Func<ContestId> dummyIdProvider = () => FixedContestId;

        // Act
        ErrorOr<Contest> result = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .Build(dummyIdProvider);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError).HasType(ErrorType.Failure)
            .And.HasCode("Illegal Stockholm format participant groups")
            .And.HasDescription("A Stockholm format contest must have no group 0 participants, " +
                                "at least three group 1 participants, and at least three group 2 participants.");
    }

    [Test]
    public async Task FluentBuilder_should_throw_given_null_group_0_participatingCountryId_arg()
    {
        // Arrange
        Action act = () => StockholmFormatContest.Create().AddGroup0Participant(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'participatingCountryId')")
            .WithParameterName("participatingCountryId");
    }

    [Test]
    public async Task FluentBuilder_should_throw_given_null_group_1_participatingCountryId_arg()
    {
        // Arrange
        Action act = () => StockholmFormatContest.Create().AddGroup1Participant(null!, ArbitraryActName, ArbitrarySongTitle);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'participatingCountryId')")
            .WithParameterName("participatingCountryId");
    }

    [Test]
    public async Task FluentBuilder_should_throw_given_null_group_2_participatingCountryId_arg()
    {
        // Arrange
        Action act = () => StockholmFormatContest.Create().AddGroup2Participant(null!, ArbitraryActName, ArbitrarySongTitle);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'participatingCountryId')")
            .WithParameterName("participatingCountryId");
    }

    [Test]
    public async Task FluentBuilder_should_throw_given_null_idProvider_arg()
    {
        // Arrange
        Action act = () => StockholmFormatContest.Create().Build(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'idProvider')")
            .WithParameterName("idProvider");
    }
}
