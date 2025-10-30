using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.Contests.TestUtils;
using Eurocentric.UnitTests.TestUtils;
using NSubstitute;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests;

public sealed partial class StockholmRulesContestTests
{
    [Test]
    [Arguments("26786949-965a-44cc-801a-22c6b5667a3b")]
    [Arguments("cbafd594-551e-4d87-8089-a8450d4ad059")]
    public async Task Builder_should_return_StockholmRulesContest_with_specified_ID(string contestIdValue)
    {
        // Arrange
        ContestId contestId = ContestId.FromValue(Guid.Parse(contestIdValue));

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(() => contestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.HasProperty(contest => contest.Id, contestId);
    }

    [Test]
    [Arguments(2016)]
    [Arguments(2025)]
    [Arguments(2050)]
    public async Task Builder_should_return_StockholmRulesContest_with_specified_ContestYear(int contestYearValue)
    {
        // Arrange
        ContestYear contestYear = ContestYear.FromValue(contestYearValue).GetValueOrDefault();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(contestYear)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(() => DefaultContestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.HasProperty(contest => contest.ContestYear, contestYear);
    }

    [Test]
    [Arguments("Stockholm")]
    [Arguments("Stockholm")]
    [Arguments("Tel Aviv")]
    public async Task Builder_should_return_StockholmRulesContest_with_specific_CityName(string cityNameValue)
    {
        // Arrange
        CityName cityName = CityName.FromValue(cityNameValue).GetValueOrDefault();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(cityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(() => DefaultContestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.HasProperty(contest => contest.CityName, cityName);
    }

    [Test]
    public async Task Builder_should_return_StockholmRulesContest_with_Stockholm_ContestRules()
    {
        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(() => DefaultContestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.HasProperty(contest => contest.ContestRules, ContestRules.Stockholm); // Act
    }

    [Test]
    public async Task Builder_should_return_StockholmRulesContest_with_false_Queryable()
    {
        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(() => DefaultContestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.HasProperty(contest => contest.Queryable, false); // Act
    }

    [Test]
    public async Task Builder_should_return_StockholmRulesContest_with_empty_ChildBroadcasts()
    {
        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(() => DefaultContestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        StockholmRulesContest contest = await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.IsNotNull();

        await Assert.That(contest.ChildBroadcasts).IsEmpty();
    }

    [Test]
    public async Task Builder_should_return_StockholmRulesContest_with_null_GlobalTelevote()
    {
        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(() => DefaultContestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        StockholmRulesContest contest = await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.IsNotNull();

        await Assert.That(contest.GlobalTelevote).IsNull();
    }

    [Test]
    public async Task Builder_should_return_StockholmRulesContest_with_specified_Participants()
    {
        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, ActName.FromValue("AT Act"), SongTitle.FromValue("AT Song"))
            .AddSemiFinal2Participant(CountryIds.Be, ActName.FromValue("BE Act"), SongTitle.FromValue("BE Song"))
            .AddSemiFinal1Participant(CountryIds.Cz, ActName.FromValue("CZ Act"), SongTitle.FromValue("CZ Song"))
            .AddSemiFinal2Participant(CountryIds.Dk, ActName.FromValue("DK Act"), SongTitle.FromValue("DK Song"))
            .AddSemiFinal1Participant(CountryIds.Ee, ActName.FromValue("EE Act"), SongTitle.FromValue("EE Song"))
            .AddSemiFinal2Participant(CountryIds.Fi, ActName.FromValue("FI Act"), SongTitle.FromValue("FI Song"))
            .Build(() => DefaultContestId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        StockholmRulesContest contest = await Assert
            .That(result.Value)
            .IsTypeOf<StockholmRulesContest>()
            .And.IsNotNull();

        await Assert
            .That(contest.Participants)
            .HasCount(6)
            .And.Contains(
                Matchers.Participant(
                    participatingCountryId: CountryIds.At,
                    semiFinalDraw: SemiFinalDraw.SemiFinal1,
                    actName: "AT Act",
                    songTitle: "AT Song"
                )
            )
            .And.Contains(
                Matchers.Participant(
                    participatingCountryId: CountryIds.Be,
                    semiFinalDraw: SemiFinalDraw.SemiFinal2,
                    actName: "BE Act",
                    songTitle: "BE Song"
                )
            )
            .And.Contains(
                Matchers.Participant(
                    participatingCountryId: CountryIds.Cz,
                    semiFinalDraw: SemiFinalDraw.SemiFinal1,
                    actName: "CZ Act",
                    songTitle: "CZ Song"
                )
            )
            .And.Contains(
                Matchers.Participant(
                    participatingCountryId: CountryIds.Dk,
                    semiFinalDraw: SemiFinalDraw.SemiFinal2,
                    actName: "DK Act",
                    songTitle: "DK Song"
                )
            )
            .And.Contains(
                Matchers.Participant(
                    participatingCountryId: CountryIds.Ee,
                    semiFinalDraw: SemiFinalDraw.SemiFinal1,
                    actName: "EE Act",
                    songTitle: "EE Song"
                )
            )
            .And.Contains(
                Matchers.Participant(
                    participatingCountryId: CountryIds.Fi,
                    semiFinalDraw: SemiFinalDraw.SemiFinal2,
                    actName: "FI Act",
                    songTitle: "FI Song"
                )
            );
    }

    [Test]
    public async Task Builder_should_fail_on_ContestYear_property_not_set()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnexpectedError>()
            .And.HasTitle("ContestYear property not set")
            .And.HasDetail("Client attempted to create a Contest aggregate without setting its ContestYear property.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_CityName_property_not_set()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnexpectedError>()
            .And.HasTitle("CityName property not set")
            .And.HasDetail("Client attempted to create a Contest aggregate without setting its CityName property.")
            .And.HasNullExtensions();
    }

    [Test]
    [Arguments(2015)]
    [Arguments(2051)]
    public async Task Builder_should_fail_on_illegal_contest_year_value(int contestYearValue)
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        Result<ContestYear, IDomainError> errorOrContestYear = ContestYear.FromValue(contestYearValue);

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(errorOrContestYear)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal contest year value")
            .And.HasDetail("Contest year value must be an integer between 2016 and 2050.")
            .And.HasExtension("contestYear", contestYearValue);
    }

    [Test]
    [Arguments("")]
    [Arguments("    ")]
    public async Task Builder_should_fail_on_illegal_city_name_value(string cityNameValue)
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        Result<CityName, IDomainError> errorOrCityName = CityName.FromValue(cityNameValue);

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(errorOrCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal city name value")
            .And.HasDetail("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasExtension("cityName", cityNameValue);
    }

    [Test]
    [Arguments("")]
    [Arguments("    ")]
    public async Task Builder_should_fail_on_illegal_act_name_value_scenario_1(string actNameValue)
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        Result<ActName, IDomainError> errorOrActName = ActName.FromValue(actNameValue);

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, errorOrActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal act name value")
            .And.HasDetail("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasExtension("actName", actNameValue);
    }

    [Test]
    [Arguments("")]
    [Arguments("    ")]
    public async Task Builder_should_fail_on_illegal_act_name_value_scenario_2(string actNameValue)
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        Result<ActName, IDomainError> errorOrActName = ActName.FromValue(actNameValue);

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, errorOrActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal act name value")
            .And.HasDetail("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasExtension("actName", actNameValue);
    }

    [Test]
    [Arguments("")]
    [Arguments("    ")]
    public async Task Builder_should_fail_on_illegal_song_title_value_scenario_1(string songTitleValue)
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        Result<SongTitle, IDomainError> errorOrSongTitle = SongTitle.FromValue(songTitleValue);

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, errorOrSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal song title value")
            .And.HasDetail(
                "Song title value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("songTitle", songTitleValue);
    }

    [Test]
    [Arguments("")]
    [Arguments("    ")]
    public async Task Builder_should_fail_on_illegal_song_title_value_scenario_2(string songTitleValue)
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        Result<SongTitle, IDomainError> errorOrSongTitle = SongTitle.FromValue(songTitleValue);

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, errorOrSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal song title value")
            .And.HasDetail(
                "Song title value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("songTitle", songTitleValue);
    }

    [Test]
    public async Task Builder_should_fail_on_participants_referencing_same_country_scenario_1()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal contest countries")
            .And.HasDetail("Each participant and global televote in a contest must reference a different country.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_participants_referencing_same_country_scenario_2()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal contest countries")
            .And.HasDetail("Each participant and global televote in a contest must reference a different country.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_participants_referencing_same_country_scenario_3()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal contest countries")
            .And.HasDetail("Each participant and global televote in a contest must reference a different country.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_non_null_global_televote()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .WithGlobalTelevote(CountryIds.Xx)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal global televote")
            .And.HasDetail(
                "A Liverpool rules contest must have a global televote. "
                    + "A Stockholm rules contest must not have a global televote."
            )
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_no_participants()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal participant counts")
            .And.HasDetail("A contest must have at least 3 participants for each semi-final draw.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_no_SemiFinal1_participants()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal participant counts")
            .And.HasDetail("A contest must have at least 3 participants for each semi-final draw.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_no_SemiFinal2_participants()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal participant counts")
            .And.HasDetail("A contest must have at least 3 participants for each semi-final draw.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_fewer_than_3_SemiFinal1_participants()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Fi, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal participant counts")
            .And.HasDetail("A contest must have at least 3 participants for each semi-final draw.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_should_fail_on_fewer_than_3_SemiFinal2_participants()
    {
        // Arrange
        IContestIdFactory idFactorySpy = Substitute.For<IContestIdFactory>();

        // Act
        Result<Contest, IDomainError> result = StockholmRulesContest
            .Create()
            .WithContestYear(ContestYear2016)
            .WithCityName(DefaultCityName)
            .AddSemiFinal1Participant(CountryIds.At, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Be, DefaultActName, DefaultSongTitle)
            .AddSemiFinal1Participant(CountryIds.Cz, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Dk, DefaultActName, DefaultSongTitle)
            .AddSemiFinal2Participant(CountryIds.Ee, DefaultActName, DefaultSongTitle)
            .Build(idFactorySpy.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert.That(() => idFactorySpy.DidNotReceive().Create()).ThrowsNothing();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal participant counts")
            .And.HasDetail("A contest must have at least 3 participants for each semi-final draw.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task Builder_WithGlobalTelevote_should_throw_given_null_countryId_arg()
    {
        // Assert
        await Assert
            .That(() => StockholmRulesContest.Create().WithGlobalTelevote(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'countryId')");
    }

    [Test]
    public async Task Builder_AddSemiFinal1Participant_should_throw_given_null_countryId_arg()
    {
        // Assert
        await Assert
            .That(() =>
                StockholmRulesContest.Create().AddSemiFinal1Participant(null!, DefaultActName, DefaultSongTitle)
            )
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'countryId')");
    }

    [Test]
    public async Task Builder_AddSemiFinal2Participant_should_throw_given_null_countryId_arg()
    {
        // Assert
        await Assert
            .That(() =>
                StockholmRulesContest.Create().AddSemiFinal2Participant(null!, DefaultActName, DefaultSongTitle)
            )
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'countryId')");
    }

    [Test]
    public async Task Builder_Build_should_throw_given_null_idProvider_arg()
    {
        // Assert
        await Assert
            .That(() => StockholmRulesContest.Create().Build(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'idProvider')");
    }
}
