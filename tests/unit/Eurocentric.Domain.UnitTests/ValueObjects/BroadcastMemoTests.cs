using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class BroadcastMemoTests : UnitTestBase
{
    public sealed class Constructor : UnitTestBase
    {
        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            const ContestStage arbitraryStage = ContestStage.SemiFinal2;
            const BroadcastStatus arbitraryStatus = BroadcastStatus.Initialized;

            // Act
            Action act = () => _ = new BroadcastMemo(null!, arbitraryStage, arbitraryStatus);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);
        }
    }

    public sealed class EqualsMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            BroadcastId sutBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStage sutStage = ContestStage.SemiFinal2;
            const BroadcastStatus sutStatus = BroadcastStatus.InProgress;

            BroadcastMemo sut = new(sutBroadcastId, sutStage, sutStatus);

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            BroadcastId sharedBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStage sharedStage = ContestStage.SemiFinal2;
            const BroadcastStatus sharedStatus = BroadcastStatus.InProgress;

            BroadcastMemo sut = new(sharedBroadcastId, sharedStage, sharedStatus);
            BroadcastMemo other = new(sharedBroadcastId, sharedStage, sharedStatus);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_broadcast_ID_values()
        {
            // Arrange
            BroadcastId sutBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            BroadcastId otherBroadcastId = BroadcastId.FromValue(Guid.Parse("acee013d-5d26-4cf8-87ac-d21cbe6eb5d7"));

            const ContestStage sharedStage = ContestStage.SemiFinal2;
            const BroadcastStatus sharedStatus = BroadcastStatus.InProgress;

            BroadcastMemo sut = new(sutBroadcastId, sharedStage, sharedStatus);
            BroadcastMemo other = new(otherBroadcastId, sharedStage, sharedStatus);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_contest_stage_values()
        {
            // Arrange
            const ContestStage sutStage = ContestStage.SemiFinal2;
            const ContestStage otherStage = ContestStage.GrandFinal;

            BroadcastId sharedBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const BroadcastStatus sharedStatus = BroadcastStatus.InProgress;

            BroadcastMemo sut = new(sharedBroadcastId, sutStage, sharedStatus);
            BroadcastMemo other = new(sharedBroadcastId, otherStage, sharedStatus);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_status_values()
        {
            // Arrange
            const BroadcastStatus sutStatus = BroadcastStatus.InProgress;
            const BroadcastStatus otherStatus = BroadcastStatus.Completed;

            BroadcastId sharedBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStage sharedStage = ContestStage.SemiFinal2;

            BroadcastMemo sut = new(sharedBroadcastId, sharedStage, sutStatus);
            BroadcastMemo other = new(sharedBroadcastId, sharedStage, otherStatus);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            BroadcastId sutBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStage sharedStage = ContestStage.SemiFinal2;
            const BroadcastStatus sutStatus = BroadcastStatus.InProgress;

            BroadcastMemo sut = new(sutBroadcastId, sharedStage, sutStatus);

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }

    public sealed class CloneAsInProgressMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_same_BroadcastId_and_ContestStage_values_and_InProgress_BroadcastStatus_value()
        {
            // Arrange
            BroadcastId sutBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));

            BroadcastMemo sut = new(sutBroadcastId, ContestStage.GrandFinal, BroadcastStatus.InProgress);

            // Act
            BroadcastMemo result = sut.CloneAsInProgress();

            // Assert
            Assert.Equal(sut.BroadcastId, result.BroadcastId);
            Assert.Equal(sut.ContestStage, result.ContestStage);
            Assert.Equal(BroadcastStatus.InProgress, result.BroadcastStatus);
        }
    }

    public sealed class CloneAsCompletedMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_same_BroadcastId_and_ContestStage_values_and_Completed_BroadcastStatus_value()
        {
            // Arrange
            BroadcastId sutBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));

            BroadcastMemo sut = new(sutBroadcastId, ContestStage.GrandFinal, BroadcastStatus.InProgress);

            // Act
            BroadcastMemo result = sut.CloneAsCompleted();

            // Assert
            Assert.Equal(sut.BroadcastId, result.BroadcastId);
            Assert.Equal(sut.ContestStage, result.ContestStage);
            Assert.Equal(BroadcastStatus.Completed, result.BroadcastStatus);
        }
    }
}
