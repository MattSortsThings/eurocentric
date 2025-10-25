using CSharpFunctionalExtensions;

namespace Eurocentric.Domain.Core;

public static class ValueTupleExtensions
{
    public static Result<ValueTuple<T1, T2>, TError> Combine<T1, T2, TError>(
        this ValueTuple<Result<T1, TError>, Result<T2, TError>> tuple
    )
    {
        (Result<T1, TError> item1, Result<T2, TError> item2) = tuple;

        if (item1.IsFailure)
        {
            return item1.Error;
        }

        if (item2.IsFailure)
        {
            return item2.Error;
        }

        return ValueTuple.Create(item1.Value, item2.Value);
    }
}
