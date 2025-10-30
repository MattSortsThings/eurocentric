using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.Contests.TestUtils;
using Eurocentric.UnitTests.TestUtils;
using NSubstitute;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests;

public sealed partial class LiverpoolRulesContestTests
{
    [Test]
    [Arguments("26786949-965a-44cc-801a-22c6b5667a3b")]
    [Arguments("cbafd594-551e-4d87-8089-a8450d4ad059")]
    public async Task CreateSemiFinal2Broadcast_should_return_Broadcast_with_specified_ID(string broadcastIdValue)
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse(broadcastIdValue));

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => broadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(result.Value).IsTypeOf<Broadcast>().And.HasProperty(broadcast => broadcast.Id, broadcastId);
    }

    [Test]
    [Arguments("2018-05-01", 2018)]
    [Arguments("2025-05-17", 2025)]
    public async Task CreateSemiFinal2Broadcast_should_return_Broadcast_with_specified_BroadcastDate(
        string broadcastDateValue,
        int contestYearValue
    )
    {
        // Arrange
        BroadcastDate broadcastDate = BroadcastDate.FromValue(DateOnly.Parse(broadcastDateValue)).GetValueOrDefault();
        ContestYear contestYear = ContestYear.FromValue(contestYearValue).GetValueOrDefault();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: contestYear,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(broadcastDate)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<Broadcast>()
            .And.HasProperty(broadcast => broadcast.BroadcastDate, broadcastDate);
    }

    [Test]
    [Arguments("26786949-965a-44cc-801a-22c6b5667a3b")]
    [Arguments("cbafd594-551e-4d87-8089-a8450d4ad059")]
    public async Task CreateSemiFinal2Broadcast_should_return_Broadcast_with_instance_ID_as_ParentContestId(
        string contestIdValue
    )
    {
        // Arrange
        ContestId contestId = ContestId.FromValue(Guid.Parse(contestIdValue));

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi],
            contestId: contestId
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<Broadcast>()
            .And.HasProperty(broadcast => broadcast.ParentContestId, contestId);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_should_return_Broadcast_with_SemiFinal2_ContestStage()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.Value)
            .IsTypeOf<Broadcast>()
            .And.HasProperty(broadcast => broadcast.ContestStage, ContestStage.SemiFinal2);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_should_return_Broadcast_with_false_Completed()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(result.Value).IsTypeOf<Broadcast>().And.HasProperty(broadcast => broadcast.Completed, false);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_specified_Competitors_scenario_1()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert
            .That(broadcast.Competitors)
            .HasCount(2)
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 1,
                    competingCountryId: CountryIds.Dk,
                    finishingPosition: 1
                )
            )
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 2,
                    competingCountryId: CountryIds.Ee,
                    finishingPosition: 2
                )
            );
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_specified_Competitors_scenario_2()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(null, CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert
            .That(broadcast.Competitors)
            .HasCount(2)
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 2,
                    competingCountryId: CountryIds.Dk,
                    finishingPosition: 1
                )
            )
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 3,
                    competingCountryId: CountryIds.Ee,
                    finishingPosition: 2
                )
            );
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_specified_Competitors_scenario_3()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, null, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert
            .That(broadcast.Competitors)
            .HasCount(2)
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 1,
                    competingCountryId: CountryIds.Dk,
                    finishingPosition: 1
                )
            )
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 3,
                    competingCountryId: CountryIds.Ee,
                    finishingPosition: 2
                )
            );
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_specified_Competitors_scenario_4()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, null, null, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert
            .That(broadcast.Competitors)
            .HasCount(2)
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 1,
                    competingCountryId: CountryIds.Dk,
                    finishingPosition: 1
                )
            )
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 4,
                    competingCountryId: CountryIds.Ee,
                    finishingPosition: 2
                )
            );
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_specified_Competitors_scenario_5()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee, null)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert
            .That(broadcast.Competitors)
            .HasCount(2)
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 1,
                    competingCountryId: CountryIds.Dk,
                    finishingPosition: 1
                )
            )
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 2,
                    competingCountryId: CountryIds.Ee,
                    finishingPosition: 2
                )
            );
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_specified_Competitors_scenario_6()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Ee, CountryIds.Fi, CountryIds.Dk)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert
            .That(broadcast.Competitors)
            .HasCount(3)
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 1,
                    competingCountryId: CountryIds.Ee,
                    finishingPosition: 1
                )
            )
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 2,
                    competingCountryId: CountryIds.Fi,
                    finishingPosition: 2
                )
            )
            .And.Contains(
                Matchers.CompetitorWithNoPointsAwards(
                    runningOrderSpot: 3,
                    competingCountryId: CountryIds.Dk,
                    finishingPosition: 3
                )
            );
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_no_Juries()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert.That(broadcast.Juries).IsEmpty();
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_return_Broadcast_with_correct_Televotes()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(() => DefaultBroadcastId);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        Broadcast broadcast = await Assert.That(result.Value).IsNotNull();

        await Assert
            .That(broadcast.Televotes)
            .HasCount(4)
            .And.Contains(Matchers.Televote(votingCountryId: CountryIds.Dk, pointsAwarded: false))
            .And.Contains(Matchers.Televote(votingCountryId: CountryIds.Ee, pointsAwarded: false))
            .And.Contains(Matchers.Televote(votingCountryId: CountryIds.Fi, pointsAwarded: false))
            .And.Contains(Matchers.Televote(votingCountryId: CountryIds.Xx, pointsAwarded: false));
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_BroadcastDate_not_set()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithCompetingCountries(CountryIds.At, CountryIds.Fi)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnexpectedError>()
            .And.HasTitle("BroadcastDate property not set")
            .And.HasDetail(
                "Client attempted to create a Broadcast aggregate without setting its BroadcastDate property."
            )
            .And.HasNullExtensions();
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_illegal_broadcast_date_value()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        DateOnly broadcastDateValue = DateOnly.Parse("1066-10-14");

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate.FromValue(broadcastDateValue))
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal broadcast date value")
            .And.HasDetail("Broadcast date value must be a date with a year between 2016 and 2050.")
            .And.HasExtension("broadcastDate", broadcastDateValue);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_illegal_competitors_count_scenario_1()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal competitors count")
            .And.HasDetail("A broadcast must have at least 2 competitors.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_illegal_competitors_count_scenario_2()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries()
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal competitors count")
            .And.HasDetail("A broadcast must have at least 2 competitors.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_illegal_competitors_count_scenario_3()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal competitors count")
            .And.HasDetail("A broadcast must have at least 2 competitors.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_illegal_competitors_count_scenario_4()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, null)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal competitors count")
            .And.HasDetail("A broadcast must have at least 2 competitors.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_illegal_competing_countries_scenario_1()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee, CountryIds.Dk)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal competing countries")
            .And.HasDetail("Each competitor in a broadcast must reference a different country.")
            .And.HasNullExtensions();
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_illegal_competing_countries_scenario_2()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, null, CountryIds.Ee, null, CountryIds.Ee)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal competing countries")
            .And.HasDetail("Each competitor in a broadcast must reference a different country.")
            .And.HasNullExtensions();
    }

    [Test]
    [MatrixDataSource]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_parent_contest_year_conflict(
        [Matrix("2020-01-01", "2022-01-01", "2024-01-01")] string broadcastDateValue,
        [Matrix(2021, 2023)] int contestYearValue
    )
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        BroadcastDate broadcastDate = BroadcastDate.FromValue(DateOnly.Parse(broadcastDateValue)).GetValueOrDefault();
        ContestYear contestYear = ContestYear.FromValue(contestYearValue).GetValueOrDefault();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: contestYear,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(broadcastDate)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Parent contest year conflict")
            .And.HasDetail("The requested contest's year does not match the provided broadcast date.")
            .And.HasExtension("broadcastDate", broadcastDateValue);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_parent_contest_participants_conflict_scenario_1()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        CountryId orphanCountryId = CountryId.FromValue(Guid.Parse("bc09624f-e722-4565-9828-93d68ddcd7d4"));

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee, orphanCountryId)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Parent contest participants conflict")
            .And.HasDetail(
                "The requested contest has no participant with the provided country ID "
                    + "eligible to compete in the provided contest stage."
            )
            .And.HasExtension("contestId", sut.Id.Value)
            .And.HasExtension("contestStage", "SemiFinal2")
            .And.HasExtension("countryId", orphanCountryId.Value);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_parent_contest_participants_conflict_scenario_2()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee, CountryIds.At)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Parent contest participants conflict")
            .And.HasDetail(
                "The requested contest has no participant with the provided country ID "
                    + "eligible to compete in the provided contest stage."
            )
            .And.HasExtension("contestId", sut.Id.Value)
            .And.HasExtension("contestStage", "SemiFinal2")
            .And.HasExtension("countryId", CountryIds.At.Value);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_fail_on_parent_contest_participants_conflict_scenario_3()
    {
        // Arrange
        IBroadcastIdFactory spyIdFactory = Substitute.For<IBroadcastIdFactory>();

        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Act
        Result<Broadcast, IDomainError> result = sut.CreateSemiFinal2Broadcast()
            .WithBroadcastDate(BroadcastDate2016JanFirst)
            .WithCompetingCountries(CountryIds.Dk, CountryIds.Ee, CountryIds.Xx)
            .Build(spyIdFactory.Create);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(() => spyIdFactory.DidNotReceiveWithAnyArgs().Create()).ThrowsNothing();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<ConflictError>()
            .And.HasTitle("Parent contest participants conflict")
            .And.HasDetail(
                "The requested contest has no participant with the provided country ID "
                    + "eligible to compete in the provided contest stage."
            )
            .And.HasExtension("contestId", sut.Id.Value)
            .And.HasExtension("contestStage", "SemiFinal2")
            .And.HasExtension("countryId", CountryIds.Xx.Value);
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_WithCompetingCountries_should_throw_given_null_competingCountryIds_arg()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        CountryId?[] nullCompetingCountries = null!;

        // Assert
        await Assert
            .That(() => sut.CreateSemiFinal2Broadcast().WithCompetingCountries(nullCompetingCountries))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'competingCountryIds')");
    }

    [Test]
    public async Task CreateSemiFinal2Broadcast_Build_should_throw_given_null_idProvider_arg()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        Func<BroadcastId> nullIdProvider = null!;

        // Assert
        await Assert
            .That(() => sut.CreateSemiFinal2Broadcast().Build(nullIdProvider))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'idProvider')");
    }
}
