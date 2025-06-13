using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.UnitTests.Broadcasts;

public class Thing
{
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
