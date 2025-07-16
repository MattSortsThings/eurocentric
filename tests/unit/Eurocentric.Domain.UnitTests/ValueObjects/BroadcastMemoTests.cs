using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public static class BroadcastMemoTests
{
    public sealed class Constructor : UnitTest
    {
        [Fact]
        public void Should_throw_given_null_broadcastId_arg()
        {
            // Arrange
            const ContestStage arbitraryStage = ContestStage.SemiFinal2;
            const bool arbitraryCompleted = true;

            // Act
            Action act = () => _ = new BroadcastMemo(null!, arbitraryStage, arbitraryCompleted);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'broadcastId')", exception.Message);
        }
    }

    public sealed class EqualsMethod : UnitTest
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            BroadcastId sutBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStage sutStage = ContestStage.SemiFinal2;
            const bool sutCompleted = true;

            BroadcastMemo sut = new(sutBroadcastId, sutStage, sutCompleted);

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
            const bool sharedCompleted = true;

            BroadcastMemo sut = new(sharedBroadcastId, sharedStage, sharedCompleted);
            BroadcastMemo other = new(sharedBroadcastId, sharedStage, sharedCompleted);

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
            const bool sharedCompleted = true;

            BroadcastMemo sut = new(sutBroadcastId, sharedStage, sharedCompleted);
            BroadcastMemo other = new(otherBroadcastId, sharedStage, sharedCompleted);

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
            const bool sharedCompleted = true;

            BroadcastMemo sut = new(sharedBroadcastId, sutStage, sharedCompleted);
            BroadcastMemo other = new(sharedBroadcastId, otherStage, sharedCompleted);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_completed_values()
        {
            // Arrange
            BroadcastId sharedBroadcastId = BroadcastId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStage sharedStage = ContestStage.SemiFinal2;

            BroadcastMemo sut = new(sharedBroadcastId, sharedStage, completed:true);
            BroadcastMemo other = new(sharedBroadcastId, sharedStage);

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
            const bool sutCompleted = true;

            BroadcastMemo sut = new(sutBroadcastId, sharedStage, sutCompleted);

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }
}
