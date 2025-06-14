using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.UnitTests.Contests;

public sealed class LiverpoolFormatContestTests : UnitTestBase
{
    public sealed class AddMemoMethod : UnitTestBase
    {
        private static LiverpoolFormatContest CreateArbitraryLiverpoolFormatContest()
        {
            ContestId fixedContestId =
                ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));

            return Contest.CreateLiverpoolFormat()
                    .WithArbitraryYearAndCity()
                    .AddGroup0Participant(TestCountryIds.Xx)
                    .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                    .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                    .Build(new FixedContestIdGenerator(fixedContestId)).Value
                as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_update_ChildBroadcasts_and_set_ContestStatus_to_InProgress_when_Initialized()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("5c4f1876-0484-4eee-b41f-e5bbb1604f0d"));

            // Assert
            Assert.Equal(ContestStatus.Initialized, sut.ContestStatus);

            Assert.Empty(sut.ChildBroadcasts);

            // Act
            sut.AddMemo(broadcastId, ContestStage.SemiFinal1);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            BroadcastMemo memo = Assert.Single(sut.ChildBroadcasts);
            Assert.Equal(broadcastId, memo.BroadcastId);
            Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
            Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
        }

        [Fact]
        public void Should_update_ChildBroadcasts_when_InProgress()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of3 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of3 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId3Of3 = BroadcastId.FromValue(Guid.Parse("03aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(broadcastId3Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            });

            // Act
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(broadcastId3Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            });
        }

        [Fact]
        public void Should_throw_given_BroadcastId_of_existing_BroadcastMemo()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of2 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of2 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of2, ContestStage.GrandFinal);

            IReadOnlyList<BroadcastMemo> existingMemos = sut.ChildBroadcasts;

            // Act
            Action act = () => sut.AddMemo(broadcastId2Of2, ContestStage.GrandFinal);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemo already exists with the provided ContestStage value.", exception.Message);

            Assert.Equal(existingMemos, sut.ChildBroadcasts);
        }

        [Fact]
        public void Should_throw_given_ContestStage_of_existing_BroadcastMemo()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId duplicateBroadcastId = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(duplicateBroadcastId, ContestStage.SemiFinal1);

            IReadOnlyList<BroadcastMemo> existingMemos = sut.ChildBroadcasts;

            // Act
            Action act = () => sut.AddMemo(duplicateBroadcastId, ContestStage.SemiFinal1);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("BroadcastMemo already exists with the provided BroadcastId value.", exception.Message);

            Assert.Equal(existingMemos, sut.ChildBroadcasts);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            // Act
            Action act = () => sut.AddMemo(null!, ContestStage.SemiFinal1);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);

            Assert.Empty(sut.ChildBroadcasts);
            Assert.Equal(ContestStatus.Initialized, sut.ContestStatus);
        }
    }

    public sealed class RemoveMemoMethod : UnitTestBase
    {
        private static LiverpoolFormatContest CreateArbitraryLiverpoolFormatContest()
        {
            ContestId fixedContestId =
                ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));

            return Contest.CreateLiverpoolFormat()
                    .WithArbitraryYearAndCity()
                    .AddGroup0Participant(TestCountryIds.Xx)
                    .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                    .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                    .Build(new FixedContestIdGenerator(fixedContestId)).Value
                as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_update_ChildBroadcasts_and_set_ContestStatus_to_Initialized_if_no_memos_remain_after_update()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("5c4f1876-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId, ContestStage.SemiFinal1);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            Assert.Single(sut.ChildBroadcasts);

            // Act
            sut.RemoveMemo(broadcastId);

            // Assert
            Assert.Empty(sut.ChildBroadcasts);
            Assert.Equal(ContestStatus.Initialized, sut.ContestStatus);
        }

        [Fact]
        public void Should_update_ChildBroadcasts_and_set_ContestStatus_to_InProgress_if_memos_remain_after_update()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of2 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of2 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of2, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of2, ContestStage.SemiFinal2);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of2, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of2, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            });

            // Act
            sut.RemoveMemo(broadcastId1Of2);

            // Assert
            BroadcastMemo remainingMemo = Assert.Single(sut.ChildBroadcasts);

            Assert.Equal(broadcastId2Of2, remainingMemo.BroadcastId);
            Assert.Equal(ContestStage.SemiFinal2, remainingMemo.ContestStage);
            Assert.Equal(BroadcastStatus.Initialized, remainingMemo.BroadcastStatus);

            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);
        }

        [Fact]
        public void Should_throw_given_BroadcastId_of_non_existent_BroadcastMemo()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of2 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of2 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of2, ContestStage.GrandFinal);

            // Act
            Action act = () => sut.RemoveMemo(broadcastId2Of2);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("No BroadcastMemo exists with the provided BroadcastId value.", exception.Message);

            Assert.Single(sut.ChildBroadcasts);
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            // Act
            Action act = () => sut.RemoveMemo(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);

            Assert.Empty(sut.ChildBroadcasts);
            Assert.Equal(ContestStatus.Initialized, sut.ContestStatus);
        }
    }

    public sealed class ReplaceMemoMethod : UnitTestBase
    {
        private static LiverpoolFormatContest CreateArbitraryLiverpoolFormatContest()
        {
            ContestId fixedContestId =
                ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));

            return Contest.CreateLiverpoolFormat()
                    .WithArbitraryYearAndCity()
                    .AddGroup0Participant(TestCountryIds.Xx)
                    .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                    .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                    .Build(new FixedContestIdGenerator(fixedContestId)).Value
                as LiverpoolFormatContest ?? throw new InvalidCastException();
        }

        [Fact]
        public void Should_update_ChildBroadcasts_and_not_update_status_if_fewer_than_3_memos()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of2 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of2 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of2, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of2, ContestStage.SemiFinal2);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of2, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of2, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            });

            // Act
            sut.ReplaceMemo(broadcastId2Of2, BroadcastStatus.InProgress);

            // Assert
            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of2, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of2, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.InProgress, memo.BroadcastStatus);
            });

            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);
        }

        [Fact]
        public void Should_update_ChildBroadcasts_and_not_update_status_if_3_memos_and_not_all_Completed_after_update()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of3 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of3 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId3Of3 = BroadcastId.FromValue(Guid.Parse("03aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            sut.ReplaceMemo(broadcastId1Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId2Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(broadcastId3Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            });

            // Act
            sut.ReplaceMemo(broadcastId3Of3, BroadcastStatus.InProgress);

            // Assert
            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(broadcastId3Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.InProgress, memo.BroadcastStatus);
            });

            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);
        }

        [Fact]
        public void Should_update_ChildBroadcasts_and_not_update_status_if_3_memos_and_all_Completed_after_update()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of3 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of3 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId3Of3 = BroadcastId.FromValue(Guid.Parse("03aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of3, ContestStage.SemiFinal1);
            sut.AddMemo(broadcastId2Of3, ContestStage.SemiFinal2);
            sut.AddMemo(broadcastId3Of3, ContestStage.GrandFinal);

            sut.ReplaceMemo(broadcastId1Of3, BroadcastStatus.Completed);
            sut.ReplaceMemo(broadcastId2Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);

            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(broadcastId3Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Initialized, memo.BroadcastStatus);
            });

            // Act
            sut.ReplaceMemo(broadcastId3Of3, BroadcastStatus.Completed);

            // Assert
            Assert.Collection(sut.ChildBroadcasts, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal1, memo.ContestStage);
                Assert.Equal(broadcastId1Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.SemiFinal2, memo.ContestStage);
                Assert.Equal(broadcastId2Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            }, ([UsedImplicitly] memo) =>
            {
                Assert.Equal(ContestStage.GrandFinal, memo.ContestStage);
                Assert.Equal(broadcastId3Of3, memo.BroadcastId);
                Assert.Equal(BroadcastStatus.Completed, memo.BroadcastStatus);
            });

            Assert.Equal(ContestStatus.Completed, sut.ContestStatus);
        }

        [Fact]
        public void Should_throw_given_BroadcastId_of_non_existent_BroadcastMemo()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            BroadcastId broadcastId1Of2 = BroadcastId.FromValue(Guid.Parse("01aabbcc-0484-4eee-b41f-e5bbb1604f0d"));
            BroadcastId broadcastId2Of2 = BroadcastId.FromValue(Guid.Parse("02aabbcc-0484-4eee-b41f-e5bbb1604f0d"));

            sut.AddMemo(broadcastId1Of2, ContestStage.GrandFinal);

            // Act
            Action act = () => sut.ReplaceMemo(broadcastId2Of2, BroadcastStatus.InProgress);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("No BroadcastMemo exists with the provided BroadcastId value.", exception.Message);

            Assert.Single(sut.ChildBroadcasts);
            Assert.Equal(ContestStatus.InProgress, sut.ContestStatus);
        }

        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            LiverpoolFormatContest sut = CreateArbitraryLiverpoolFormatContest();

            // Act
            Action act = () => sut.ReplaceMemo(null!, BroadcastStatus.InProgress);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);

            Assert.Empty(sut.ChildBroadcasts);
            Assert.Equal(ContestStatus.Initialized, sut.ContestStatus);
        }
    }

    public sealed class CreateSemiFinal1ChildBroadcastMethod : UnitTestBase
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
        public void Should_return_Broadcast_with_Initialized_BroadcastStatus_value_and_SemiFinal1_ContestStatus_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(BroadcastStatus.Initialized, broadcast.BroadcastStatus);
            Assert.Equal(ContestStage.SemiFinal1, broadcast.ContestStage);
        }

        [Fact]
        public void Should_return_Broadcast_with_instance_Id_value_as_ParentContestId_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(sut.Id, broadcast.ParentContestId);
        }

        [Fact]
        public void Should_return_Broadcast_with_provided_BroadcastDate_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            BroadcastDate broadcastDate = BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-05", "yyyy-mm-dd")).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(broadcastDate, broadcast.BroadcastDate);
        }

        [Fact]
        public void Should_return_Broadcast_with_provided_Competitors_all_with_empty_JuryAwards_and_empty_TelevoteAwards()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            List<CountryId> competingCountryIds = [TestCountryIds.Cz, TestCountryIds.At, TestCountryIds.Be];

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(competingCountryIds)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(competingCountryIds, broadcast.Competitors.Select(competitor => competitor.CompetingCountryId));
            Assert.All(broadcast.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(broadcast.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
        }

        [Fact]
        public void Should_return_Broadcast_with_no_Juries()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Empty(broadcast.Juries);
        }

        [Fact]
        public void Should_return_Broadcast_with_Televote_for_every_group_0_and_group_1_Participant_all_PointsAwarded_false()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId[] expectedTelevotingCountryIds =
            [
                TestCountryIds.Xx, TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz
            ];

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(expectedTelevotingCountryIds, broadcast.Televotes.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_when_instance_has_ChildBroadcast_with_SemiFinal1_ContestStage_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("1f2036e8-318c-4a98-8806-62c1bf4c3f55"));

            sut.AddMemo(existingBroadcastId, ContestStage.SemiFinal1);

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) =
                (errorsOrBroadcast.IsError, errorsOrBroadcast.Value, errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast contest stage conflict", firstError.Code);
            Assert.Equal("The contest already has a child broadcast with the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.SemiFinal1 });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_BroadcastDate_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            DateOnly illegalBroadcastDateValue = DateOnly.ParseExact("1999-01-01", "yyyy-MM-dd");

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate.FromValue(illegalBroadcastDateValue))
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast date value", firstError.Code);
            Assert.Equal("Broadcast date value must be between 2016-01-01 and 2050-12-31.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d }
                                                        && d == illegalBroadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_BroadcastDate_value_outside_ContestYear_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            DateOnly wrongYearBroadcastDateValue = DateOnly.ParseExact("2024-12-31", "yyyy-MM-dd");

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate.FromValue(wrongYearBroadcastDateValue))
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Broadcast date out of range", firstError.Code);
            Assert.Equal("A broadcast's date must be in the same year as its parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d }
                                                        && d == wrongYearBroadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_2_competing_country_IDs()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Insufficient competitors", firstError.Code);
            Assert.Equal("A broadcast must have at least 2 competitors.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_competing_country_IDs()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.At)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing countries", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must reference a different competing country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_no_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId orphanCountryId = CountryId.FromValue(Guid.Parse("2b2808e6-c305-4c8c-ae2e-2fa7e890dc2f"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, orphanCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Orphan competitor", firstError.Code);
            Assert.Equal("Parent contest has no participant with the provided competing country ID.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == orphanCountryId.Value);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_group_0_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId ineligibleCountryId = TestCountryIds.Xx;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, ineligibleCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Ineligible competing country", firstError.Code);
            Assert.Equal("The contest has a participant with the provided competing country ID, " +
                         "but they are not eligible to compete in the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == ineligibleCountryId.Value);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.SemiFinal1 });
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_group_2_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId ineligibleCountryId = TestCountryIds.Fi;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal1ChildBroadcast()
                .WithBroadcastDate(BroadcastDate1May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Be, ineligibleCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Ineligible competing country", firstError.Code);
            Assert.Equal("The contest has a participant with the provided competing country ID, " +
                         "but they are not eligible to compete in the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == ineligibleCountryId.Value);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.SemiFinal1 });
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            Action act = () => sut.CreateSemiFinal1ChildBroadcast().WithCompetingCountryIds(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idGenerator_arg()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            Action act = () => sut.CreateSemiFinal1ChildBroadcast().Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idGenerator')", exception.Message);
        }

        private sealed class DummyBroadcastIdGenerator : IBroadcastIdGenerator
        {
            public BroadcastId Generate() => BroadcastId.FromValue(Guid.Empty);
        }
    }

    public sealed class CreateSemiFinal2ChildBroadcastMethod : UnitTestBase
    {
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("bf5a7250-3a6b-456f-a3f8-d8280851afff"));
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
        private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;
        private static readonly BroadcastDate BroadcastDate2May2025 =
            BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-02", "yyyy-mm-dd")).Value;

        [Fact]
        public void Should_return_Broadcast_with_Initialized_BroadcastStatus_value_and_SemiFinal2_ContestStatus_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(BroadcastStatus.Initialized, broadcast.BroadcastStatus);
            Assert.Equal(ContestStage.SemiFinal2, broadcast.ContestStage);
        }

        [Fact]
        public void Should_return_Broadcast_with_instance_Id_value_as_ParentContestId_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(sut.Id, broadcast.ParentContestId);
        }

        [Fact]
        public void Should_return_Broadcast_with_provided_BroadcastDate_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            BroadcastDate broadcastDate = BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-05", "yyyy-mm-dd")).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(broadcastDate, broadcast.BroadcastDate);
        }

        [Fact]
        public void Should_return_Broadcast_with_provided_Competitors_all_with_empty_JuryAwards_and_empty_TelevoteAwards()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            List<CountryId> competingCountryIds = [TestCountryIds.Fi, TestCountryIds.Dk, TestCountryIds.Ee];

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(competingCountryIds)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(competingCountryIds, broadcast.Competitors.Select(competitor => competitor.CompetingCountryId));
            Assert.All(broadcast.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(broadcast.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
        }

        [Fact]
        public void Should_return_Broadcast_with_no_Juries()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Empty(broadcast.Juries);
        }

        [Fact]
        public void Should_return_Broadcast_with_Televote_for_every_group_0_and_group_2_Participant_all_PointsAwarded_false()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId[] expectedTelevotingCountryIds =
            [
                TestCountryIds.Xx, TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi
            ];

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(expectedTelevotingCountryIds, broadcast.Televotes.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_when_instance_has_ChildBroadcast_with_SemiFinal2_ContestStage_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("1f2036e8-318c-4a98-8806-62c1bf4c3f55"));

            sut.AddMemo(existingBroadcastId, ContestStage.SemiFinal2);

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) =
                (errorsOrBroadcast.IsError, errorsOrBroadcast.Value, errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast contest stage conflict", firstError.Code);
            Assert.Equal("The contest already has a child broadcast with the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.SemiFinal2 });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_BroadcastDate_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            DateOnly illegalBroadcastDateValue = DateOnly.ParseExact("1999-01-01", "yyyy-MM-dd");

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate.FromValue(illegalBroadcastDateValue))
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast date value", firstError.Code);
            Assert.Equal("Broadcast date value must be between 2016-01-01 and 2050-12-31.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d }
                                                        && d == illegalBroadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_BroadcastDate_value_outside_ContestYear_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            DateOnly wrongYearBroadcastDateValue = DateOnly.ParseExact("2024-12-31", "yyyy-MM-dd");

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate.FromValue(wrongYearBroadcastDateValue))
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Broadcast date out of range", firstError.Code);
            Assert.Equal("A broadcast's date must be in the same year as its parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d }
                                                        && d == wrongYearBroadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_2_competing_country_IDs()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Insufficient competitors", firstError.Code);
            Assert.Equal("A broadcast must have at least 2 competitors.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_competing_country_IDs()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Dk)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) =
                (errorsOrBroadcast.IsError, errorsOrBroadcast.Value, errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing countries", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must reference a different competing country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_no_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId orphanCountryId = CountryId.FromValue(Guid.Parse("2b2808e6-c305-4c8c-ae2e-2fa7e890dc2f"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee, orphanCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Orphan competitor", firstError.Code);
            Assert.Equal("Parent contest has no participant with the provided competing country ID.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == orphanCountryId.Value);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_group_0_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId ineligibleCountryId = TestCountryIds.Xx;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee, ineligibleCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Ineligible competing country", firstError.Code);
            Assert.Equal("The contest has a participant with the provided competing country ID, " +
                         "but they are not eligible to compete in the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == ineligibleCountryId.Value);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.SemiFinal2 });
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_group_1_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId ineligibleCountryId = TestCountryIds.Cz;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateSemiFinal2ChildBroadcast()
                .WithBroadcastDate(BroadcastDate2May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee, ineligibleCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Ineligible competing country", firstError.Code);
            Assert.Equal("The contest has a participant with the provided competing country ID, " +
                         "but they are not eligible to compete in the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == ineligibleCountryId.Value);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.SemiFinal2 });
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            Action act = () => sut.CreateSemiFinal2ChildBroadcast().WithCompetingCountryIds(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idGenerator_arg()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            Action act = () => sut.CreateSemiFinal2ChildBroadcast().Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idGenerator')", exception.Message);
        }

        private sealed class DummyBroadcastIdGenerator : IBroadcastIdGenerator
        {
            public BroadcastId Generate() => BroadcastId.FromValue(Guid.Empty);
        }
    }

    public sealed class CreateGrandFinalChildBroadcastMethod : UnitTestBase
    {
        private static readonly BroadcastId FixedBroadcastId =
            BroadcastId.FromValue(Guid.Parse("bf5a7250-3a6b-456f-a3f8-d8280851afff"));
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("e93dea75-06e5-4b66-b7c1-2ce794325db3"));
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
        private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;
        private static readonly BroadcastDate BroadcastDate3May2025 =
            BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-03", "yyyy-mm-dd")).Value;

        [Fact]
        public void Should_return_Broadcast_with_Initialized_BroadcastStatus_value_and_GrandFinal_ContestStatus_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(BroadcastStatus.Initialized, broadcast.BroadcastStatus);
            Assert.Equal(ContestStage.GrandFinal, broadcast.ContestStage);
        }

        [Fact]
        public void Should_return_Broadcast_with_instance_Id_value_as_ParentContestId_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(sut.Id, broadcast.ParentContestId);
        }

        [Fact]
        public void Should_return_Broadcast_with_provided_BroadcastDate_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            BroadcastDate broadcastDate = BroadcastDate.FromValue(DateOnly.ParseExact("2025-05-05", "yyyy-mm-dd")).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(broadcastDate)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(broadcastDate, broadcast.BroadcastDate);
        }

        [Fact]
        public void Should_return_Broadcast_with_provided_Competitors_all_with_empty_JuryAwards_and_empty_TelevoteAwards()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            List<CountryId> competingCountryIds =
            [
                TestCountryIds.At, TestCountryIds.Fi, TestCountryIds.Cz, TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Be
            ];

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(competingCountryIds)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equal(competingCountryIds, broadcast.Competitors.Select(competitor => competitor.CompetingCountryId));
            Assert.All(broadcast.Competitors, competitor => Assert.Empty(competitor.JuryAwards));
            Assert.All(broadcast.Competitors, competitor => Assert.Empty(competitor.TelevoteAwards));
        }

        [Fact]
        public void Should_return_Broadcast_with_Jury_for_every_group_1_and_group_2_Participant_all_PointsAwarded_false()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId[] expectedJuryVotingCountryIds =
            [
                TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz, TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi
            ];

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(expectedJuryVotingCountryIds, broadcast.Juries.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Juries, jury => Assert.False(jury.PointsAwarded));
        }

        [Fact]
        public void Should_return_Broadcast_with_Televote_for_every_Participant_all_PointsAwarded_false()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId[] expectedTelevotingCountryIds =
            [
                TestCountryIds.Xx, TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz, TestCountryIds.Dk, TestCountryIds.Ee,
                TestCountryIds.Fi
            ];

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new FixedBroadcastIdGenerator(FixedBroadcastId));

            (bool isError, Broadcast broadcast) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(broadcast);

            Assert.Equivalent(expectedTelevotingCountryIds, broadcast.Televotes.Select(voter => voter.VotingCountryId));
            Assert.All(broadcast.Televotes, televote => Assert.False(televote.PointsAwarded));
        }

        [Fact]
        public void Should_return_Errors_when_instance_has_ChildBroadcast_with_GrandFinal_ContestStage_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("1f2036e8-318c-4a98-8806-62c1bf4c3f55"));

            sut.AddMemo(existingBroadcastId, ContestStage.GrandFinal);

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) =
                (errorsOrBroadcast.IsError, errorsOrBroadcast.Value, errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Child broadcast contest stage conflict", firstError.Code);
            Assert.Equal("The contest already has a child broadcast with the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.GrandFinal });
        }

        [Fact]
        public void Should_return_Errors_given_illegal_BroadcastDate_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            DateOnly illegalBroadcastDateValue = DateOnly.ParseExact("1999-01-01", "yyyy-MM-dd");

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate.FromValue(illegalBroadcastDateValue))
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal broadcast date value", firstError.Code);
            Assert.Equal("Broadcast date value must be between 2016-01-01 and 2050-12-31.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d }
                                                        && d == illegalBroadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_BroadcastDate_value_outside_ContestYear_value()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            DateOnly wrongYearBroadcastDateValue = DateOnly.ParseExact("2024-12-31", "yyyy-MM-dd");

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate.FromValue(wrongYearBroadcastDateValue))
                .WithCompetingCountryIds(TestCountryIds.At, TestCountryIds.Dk)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Broadcast date out of range", firstError.Code);
            Assert.Equal("A broadcast's date must be in the same year as its parent contest.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d }
                                                        && d == wrongYearBroadcastDateValue);
        }

        [Fact]
        public void Should_return_Errors_given_fewer_than_2_competing_country_IDs()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Insufficient competitors", firstError.Code);
            Assert.Equal("A broadcast must have at least 2 competitors.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_Errors_given_duplicate_competing_country_IDs()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Dk)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) =
                (errorsOrBroadcast.IsError, errorsOrBroadcast.Value, errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate competing countries", firstError.Code);
            Assert.Equal("Every competitor in a broadcast must reference a different competing country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_no_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId orphanCountryId = CountryId.FromValue(Guid.Parse("2b2808e6-c305-4c8c-ae2e-2fa7e890dc2f"));

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee, orphanCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Orphan competitor", firstError.Code);
            Assert.Equal("Parent contest has no participant with the provided competing country ID.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == orphanCountryId.Value);
        }

        [Fact]
        public void Should_return_Errors_given_competing_country_ID_matching_group_0_Participant()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            CountryId ineligibleCountryId = TestCountryIds.Xx;

            // Act
            ErrorOr<Broadcast> errorsOrBroadcast = sut.CreateGrandFinalChildBroadcast()
                .WithBroadcastDate(BroadcastDate3May2025)
                .WithCompetingCountryIds(TestCountryIds.Dk, TestCountryIds.Ee, ineligibleCountryId)
                .Build(new DummyBroadcastIdGenerator());

            (bool isError, Broadcast broadcast, Error firstError) = (errorsOrBroadcast.IsError, errorsOrBroadcast.Value,
                errorsOrBroadcast.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(broadcast);

            Assert.Equal(ErrorType.Conflict, firstError.Type);
            Assert.Equal("Ineligible competing country", firstError.Code);
            Assert.Equal("The contest has a participant with the provided competing country ID, " +
                         "but they are not eligible to compete in the provided contest stage.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "competingCountryId", Value: Guid g }
                                                        && g == ineligibleCountryId.Value);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestStage", Value: ContestStage.GrandFinal });
        }

        [Fact]
        public void Should_throw_given_null_competingCountryIds_arg()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            Action act = () => sut.CreateGrandFinalChildBroadcast().WithCompetingCountryIds(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'competingCountryIds')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idGenerator_arg()
        {
            // Arrange
            Contest sut = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear2025)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .WithGroup1Countries(TestCountryIds.At, TestCountryIds.Be, TestCountryIds.Cz)
                .WithGroup2Countries(TestCountryIds.Dk, TestCountryIds.Ee, TestCountryIds.Fi)
                .Build(new FixedContestIdGenerator(FixedContestId)).Value;

            // Act
            Action act = () => sut.CreateGrandFinalChildBroadcast().Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idGenerator')", exception.Message);
        }

        private sealed class DummyBroadcastIdGenerator : IBroadcastIdGenerator
        {
            public BroadcastId Generate() => BroadcastId.FromValue(Guid.Empty);
        }
    }

    public sealed class FluentBuilder : UnitTestBase
    {
        private static readonly ContestId FixedContestId =
            ContestId.FromValue(Guid.Parse("11ade11e-4b09-42eb-86c3-ad4123ab645f"));

        private static readonly ContestYear ArbitraryContestYear = ContestYear.FromValue(2025).Value;
        private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
        private static readonly ActName ArbitraryActName = ActName.FromValue("ActName").Value;
        private static readonly SongTitle ArbitrarySongTitle = SongTitle.FromValue("SongTitle").Value;

        [Theory]
        [InlineData(2023, "Liverpool")]
        [InlineData(2025, "Basel")]
        public void Should_create_contest_given_legal_args_scenario(int contestYear, string cityName)
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear.FromValue(contestYear))
                .WithCityName(CityName.FromValue(cityName))
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ActName.FromValue("Act AT"), SongTitle.FromValue("Song AT"))
                .AddGroup1Participant(TestCountryIds.Be, ActName.FromValue("Act BE"), SongTitle.FromValue("Song BE"))
                .AddGroup1Participant(TestCountryIds.Cz, ActName.FromValue("Act CZ"), SongTitle.FromValue("Song CZ"))
                .AddGroup2Participant(TestCountryIds.Dk, ActName.FromValue("Act DK"), SongTitle.FromValue("Song DK"))
                .AddGroup2Participant(TestCountryIds.Ee, ActName.FromValue("Act EE"), SongTitle.FromValue("Song EE"))
                .AddGroup2Participant(TestCountryIds.Fi, ActName.FromValue("Act FI"), SongTitle.FromValue("Song FI"))
                .Build(new FixedContestIdGenerator(FixedContestId));

            (bool isError, Contest contest) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            LiverpoolFormatContest createdContest = Assert.IsType<LiverpoolFormatContest>(contest);

            Assert.Equal(FixedContestId, createdContest.Id);
            Assert.Equal(contestYear, createdContest.ContestYear.Value);
            Assert.Equal(cityName, createdContest.CityName.Value);
            Assert.Equal(ContestFormat.Liverpool, createdContest.ContestFormat);
            Assert.Equal(ContestStatus.Initialized, createdContest.ContestStatus);
            Assert.Empty(createdContest.ChildBroadcasts);

            Assert.Collection(createdContest.Participants, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Zero, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Xx, participant.ParticipatingCountryId);
                Assert.Null(participant.ActName);
                Assert.Null(participant.SongTitle);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.At, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act AT", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song AT", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Be, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act BE", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song BE", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.One, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Cz, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act CZ", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song CZ", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Dk, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act DK", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song DK", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Ee, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act EE", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song EE", participant.SongTitle.Value);
            }, ([UsedImplicitly] participant) =>
            {
                Assert.Equal(ParticipantGroup.Two, participant.ParticipantGroup);
                Assert.Equal(TestCountryIds.Fi, participant.ParticipatingCountryId);
                Assert.NotNull(participant.ActName);
                Assert.Equal("Act FI", participant.ActName.Value);
                Assert.NotNull(participant.SongTitle);
                Assert.Equal("Song FI", participant.SongTitle.Value);
            });
        }

        [Fact]
        public void Should_return_errors_given_illegal_contest_year_value()
        {
            // Arrange
            const int illegalContestYearValue = 999999;

            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .WithContestYear(ContestYear.FromValue(illegalContestYearValue))
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal contest year value", firstError.Code);
            Assert.Equal("Contest year value must be an integer between 2016 and 2050.", firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "contestYear", Value: illegalContestYearValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_city_name_value()
        {
            // Arrange
            const string illegalCityNameValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .WithCityName(CityName.FromValue(illegalCityNameValue))
                .WithContestYear(ArbitraryContestYear)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal("City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "cityName", Value: illegalCityNameValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_1_participant_act_name_value()
        {
            // Arrange
            const string illegalActNameValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup1Participant(TestCountryIds.At, ActName.FromValue(illegalActNameValue), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: illegalActNameValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_2_participant_act_name_value()
        {
            // Arrange
            const string illegalActNameValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup2Participant(TestCountryIds.Fi, ActName.FromValue(illegalActNameValue), ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal act name value", firstError.Code);
            Assert.Equal("Act name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "actName", Value: illegalActNameValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_1_participant_song_title_value()
        {
            // Arrange
            const string illegalSongTitleValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, SongTitle.FromValue(illegalSongTitleValue))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: illegalSongTitleValue });
        }

        [Fact]
        public void Should_return_errors_given_illegal_group_2_participant_song_title_value()
        {
            // Arrange
            const string illegalSongTitleValue = "";

            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, SongTitle.FromValue(illegalSongTitleValue))
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal song title value", firstError.Code);
            Assert.Equal("Song title value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.NotNull(firstError.Metadata);
            Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "songTitle", Value: illegalSongTitleValue });
        }

        [Fact]
        public void Should_return_errors_given_zero_group_0_participants()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single participant in group 0, " +
                         "at least 3 in group 1, and at least 3 in group 2.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_multiple_group_0_participants()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup0Participant(TestCountryIds.Gb)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single participant in group 0, " +
                         "at least 3 in group 1, and at least 3 in group 2.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_3_group_1_participants()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single participant in group 0, " +
                         "at least 3 in group 1, and at least 3 in group 2.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_fewer_than_3_group_2_participants()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Illegal Liverpool format group sizes", firstError.Code);
            Assert.Equal("A Liverpool format contest must have a single participant in group 0, " +
                         "at least 3 in group 1, and at least 3 in group 2.", firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_1_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup1Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_2_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup2Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_0_and_1_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup0Participant(TestCountryIds.Gb)
                .AddGroup1Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_0_and_2_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup0Participant(TestCountryIds.Gb)
                .AddGroup2Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_return_errors_given_group_1_and_2_participants_from_same_country()
        {
            // Act
            ErrorOr<Contest> result = Contest.CreateLiverpoolFormat()
                .AddGroup1Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Gb, ArbitraryActName, ArbitrarySongTitle)
                .WithContestYear(ArbitraryContestYear)
                .WithCityName(ArbitraryCityName)
                .AddGroup0Participant(TestCountryIds.Xx)
                .AddGroup1Participant(TestCountryIds.At, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Be, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup1Participant(TestCountryIds.Cz, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Dk, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Ee, ArbitraryActName, ArbitrarySongTitle)
                .AddGroup2Participant(TestCountryIds.Fi, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            (bool isError, Contest contest, Error firstError) = (result.IsError, result.Value, result.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(contest);

            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.Equal("Duplicate participating countries", firstError.Code);
            Assert.Equal("Every participant in a contest must reference a different participating country.",
                firstError.Description);
            Assert.Null(firstError.Metadata);
        }

        [Fact]
        public void Should_throw_given_null_group_0_participant_countryId_arg()
        {
            // Act
            Action act = () => Contest.CreateLiverpoolFormat()
                .AddGroup0Participant(null!)
                .Build(new DummyContestIdGenerator());

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_1_participant_countryId_arg()
        {
            // Act
            Action act = () => Contest.CreateLiverpoolFormat()
                .AddGroup1Participant(null!, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_group_2_participant_countryId_arg()
        {
            // Act
            Action act = () => Contest.CreateLiverpoolFormat()
                .AddGroup2Participant(null!, ArbitraryActName, ArbitrarySongTitle)
                .Build(new DummyContestIdGenerator());

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'countryId')", exception.Message);
        }

        [Fact]
        public void Should_throw_given_null_idGenerator_arg()
        {
            // Act
            Action act = () => Contest.CreateLiverpoolFormat()
                .Build(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'idGenerator')", exception.Message);
        }

        private sealed class DummyContestIdGenerator : IContestIdGenerator
        {
            public ContestId Generate() => FixedContestId;
        }
    }
}
