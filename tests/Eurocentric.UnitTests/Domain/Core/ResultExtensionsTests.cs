using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.Core;

public sealed class ResultExtensionsTests : UnitTest
{
    private static readonly TestItem FixedValue = new();

    private static readonly IDomainError FixedError = new UnprocessableError
    {
        Title = "Fixed error",
        Detail = "Fixed error for unit tests",
        Extensions = null,
    };

    private static UnitResult<IDomainError> PredicateThatAlwaysPasses(TestItem value) =>
        UnitResult.Success<IDomainError>();

    private static Func<TestItem, UnitResult<IDomainError>> PredicateThatAlwaysFailsWithError(IDomainError error) =>
        _ => UnitResult.Failure(error);

    [Test]
    public async Task Ensure_Result_should_return_Failure_Result_when_it_is_Failure()
    {
        // Arrange
        Result<TestItem, IDomainError> sut = Result.Failure<TestItem, IDomainError>(FixedError);

        // Assert
        await Assert.That(sut.IsFailure).IsTrue();

        // Act
        Result<TestItem, IDomainError> result = sut.Ensure(PredicateThatAlwaysPasses);

        // Assert
        await Assert.That(result).IsNotSameReferenceAs(sut);
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error).IsSameReferenceAs(FixedError);
        await Assert.That(result.GetValueOrDefault()).IsNull();
    }

    [Test]
    public async Task Ensure_Result_should_return_Success_Result_when_it_is_Success_and_predicate_passes()
    {
        // Arrange
        Result<TestItem, IDomainError> sut = Result.Success<TestItem, IDomainError>(FixedValue);

        // Assert
        await Assert.That(sut.IsSuccess).IsTrue();

        // Act
        Result<TestItem, IDomainError> result = sut.Ensure(PredicateThatAlwaysPasses);

        // Assert
        await Assert.That(result).IsNotSameReferenceAs(sut);
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.GetValueOrDefault()).IsSameReferenceAs(FixedValue);
    }

    [Test]
    public async Task Ensure_Result_should_return_Failure_Result_when_it_is_Success_and_predicate_fails()
    {
        // Arrange
        Result<TestItem, IDomainError> sut = Result.Success<TestItem, IDomainError>(FixedValue);

        // Assert
        await Assert.That(sut.IsSuccess).IsTrue();

        // Act
        Result<TestItem, IDomainError> result = sut.Ensure(PredicateThatAlwaysFailsWithError(FixedError));

        // Assert
        await Assert.That(result).IsNotSameReferenceAs(sut);
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error).IsSameReferenceAs(FixedError);
        await Assert.That(result.GetValueOrDefault()).IsNull();
    }

    [Test]
    public async Task Ensure_Task_Result_should_return_Failure_Result_when_it_is_Failure()
    {
        // Arrange
        Result<TestItem, IDomainError> sut = Result.Failure<TestItem, IDomainError>(FixedError);

        Task<Result<TestItem, IDomainError>> sutTask = Task.FromResult(sut);

        // Assert
        await Assert.That(sut.IsFailure).IsTrue();

        // Act
        Result<TestItem, IDomainError> result = await sutTask.Ensure(PredicateThatAlwaysPasses);

        // Assert
        await Assert.That(result).IsNotSameReferenceAs(sut);
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error).IsSameReferenceAs(FixedError);
        await Assert.That(result.GetValueOrDefault()).IsNull();
    }

    [Test]
    public async Task Ensure_Task_Result_should_return_Success_Result_when_it_is_Success_and_predicate_passes()
    {
        // Arrange
        Result<TestItem, IDomainError> sut = Result.Success<TestItem, IDomainError>(FixedValue);

        Task<Result<TestItem, IDomainError>> sutTask = Task.FromResult(sut);

        // Assert
        await Assert.That(sut.IsSuccess).IsTrue();

        // Act
        Result<TestItem, IDomainError> result = await sutTask.Ensure(PredicateThatAlwaysPasses);

        // Assert
        await Assert.That(result).IsNotSameReferenceAs(sut);
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.GetValueOrDefault()).IsSameReferenceAs(FixedValue);
    }

    [Test]
    public async Task Ensure_Task_Result_should_return_Failure_Result_when_it_is_Success_and_predicate_fails()
    {
        // Arrange
        Result<TestItem, IDomainError> sut = Result.Success<TestItem, IDomainError>(FixedValue);

        Task<Result<TestItem, IDomainError>> sutTask = Task.FromResult(sut);

        // Assert
        await Assert.That(sut.IsSuccess).IsTrue();

        // Act
        Result<TestItem, IDomainError> result = await sutTask.Ensure(PredicateThatAlwaysFailsWithError(FixedError));

        // Assert
        await Assert.That(result).IsNotSameReferenceAs(sut);
        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error).IsSameReferenceAs(FixedError);
        await Assert.That(result.GetValueOrDefault()).IsNull();
    }

    private sealed record TestItem;
}
