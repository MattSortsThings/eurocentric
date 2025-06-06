using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class ContestMemoTests : UnitTestBase
{
    public sealed class Constructor : UnitTestBase
    {
        [Fact]
        public void Should_throw_given_null_contestId_arg()
        {
            // Arrange
            const ContestStatus arbitraryStatus = ContestStatus.Initialized;

            // Act
            Action act = () => _ = new ContestMemo(null!, arbitraryStatus);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'contestId')", exception.Message);
        }
    }

    public sealed class EqualsMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            ContestId sutContestId = ContestId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStatus sutStatus = ContestStatus.InProgress;

            ContestMemo sut = new(sutContestId, sutStatus);

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            ContestId sharedContestId = ContestId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStatus sharedStatus = ContestStatus.InProgress;

            ContestMemo sut = new(sharedContestId, sharedStatus);
            ContestMemo other = new(sharedContestId, sharedStatus);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_ContestId_values()
        {
            // Arrange
            ContestId sutContestId = ContestId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            ContestId otherContestId = ContestId.FromValue(Guid.Parse("acee013d-5d26-4cf8-87ac-d21cbe6eb5d7"));
            const ContestStatus sharedStatus = ContestStatus.InProgress;

            ContestMemo sut = new(sutContestId, sharedStatus);
            ContestMemo other = new(otherContestId, sharedStatus);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_ContestStatus_values()
        {
            // Arrange
            ContestId sharedContestId = ContestId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStatus sutStatus = ContestStatus.InProgress;
            const ContestStatus otherStatus = ContestStatus.Completed;

            ContestMemo sut = new(sharedContestId, sutStatus);
            ContestMemo other = new(sharedContestId, otherStatus);

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            ContestId sutContestId = ContestId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            const ContestStatus sutStatus = ContestStatus.InProgress;

            ContestMemo sut = new(sutContestId, sutStatus);

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }

    public sealed class CloneAsInProgressMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_same_ContestId_value_and_InProgress_ContestStatus_value()
        {
            // Arrange
            ContestId sutContestId = ContestId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            ContestMemo sut = new(sutContestId, ContestStatus.Initialized);

            // Act
            ContestMemo result = sut.CloneAsInProgress();

            // Assert
            Assert.Equal(sut.ContestId, result.ContestId);
            Assert.Equal(ContestStatus.InProgress, result.ContestStatus);
        }
    }

    public sealed class CloneAsCompletedMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_instance_with_same_ContestId_value_and_Completed_ContestStatus_value()
        {
            // Arrange
            ContestId sutContestId = ContestId.FromValue(Guid.Parse("7dd8f418-30ec-46e4-9145-dfe9e648ea57"));
            ContestMemo sut = new(sutContestId, ContestStatus.Initialized);

            // Act
            ContestMemo result = sut.CloneAsCompleted();

            // Assert
            Assert.Equal(sut.ContestId, result.ContestId);
            Assert.Equal(ContestStatus.Completed, result.ContestStatus);
        }
    }
}
