using CSharpFunctionalExtensions;

namespace Eurocentric.Domain.Functional;

public static class ResultExtensions
{
    public static Result<TValue, TError> Ensure<TValue, TError>(
        this Result<TValue, TError> result,
        Func<TValue, UnitResult<TError>> func
    )
    {
        return result.IsFailure ? result
            : func(result.GetValueOrDefault()) is { IsFailure: true } funcResult ? funcResult.Error
            : result;
    }

    public static async Task<Result<TValue, TError>> Ensure<TValue, TError>(
        this Task<Result<TValue, TError>> resultTask,
        Func<TValue, UnitResult<TError>> func
    )
    {
        Result<TValue, TError> result = await resultTask.ConfigureAwait(false);

        return result.Ensure(func);
    }
}
