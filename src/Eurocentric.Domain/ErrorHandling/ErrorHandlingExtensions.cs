using ErrorOr;

namespace Eurocentric.Domain.ErrorHandling;

/// <summary>
///     Convenience methods for the <see cref="ErrorOr" /> library.
/// </summary>
public static class ErrorHandlingExtensions
{
    /// <summary>
    ///     Collects a sequence of errors or values into either a list of all their values when all are successful or else a
    ///     list of all their errors.
    /// </summary>
    /// <param name="errorsOrValues">A sequence of <see cref="ErrorOr{T}" /> values.</param>
    /// <typeparam name="TValue">The successful value type.</typeparam>
    /// <returns>
    ///     A <see cref="List{T}" /> of type <typeparamref name="TValue" /> when all elements of the
    ///     <paramref name="errorsOrValues" /> argument are successful; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    public static ErrorOr<List<TValue>> Collect<TValue>(this IEnumerable<ErrorOr<TValue>> errorsOrValues)
    {
        List<TValue> values = [];

        List<Error> errors = [];

        foreach (ErrorOr<TValue> errorsOrValue in errorsOrValues)
        {
            if (errorsOrValue.IsError)
            {
                errors.AddRange(errorsOrValue.Errors);
            }
            else
            {
                values.Add(errorsOrValue.Value);
            }
        }

        return errors.Count > 0 ? errors : values;
    }

    /// <summary>
    ///     Combines a 2-tuple of errors or values into either a 2-tuple of their values when both are successful or else a
    ///     list of all their errors.
    /// </summary>
    /// <param name="tuple">A 2-tuple of <see cref="ErrorOr{T}" /> values.</param>
    /// <typeparam name="T1">The first value type of the 2-tuple.</typeparam>
    /// <typeparam name="T2">The second value type of the 2-tuple.</typeparam>
    /// <returns>
    ///     A <see cref="Tuple{T1,T2}" /> composed of the <paramref name="tuple" /> member values when both are
    ///     successful; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    public static ErrorOr<Tuple<T1, T2>> Combine<T1, T2>(this Tuple<ErrorOr<T1>, ErrorOr<T2>> tuple)
    {
        var (t1, t2) = tuple;

        List<Error> errors = new(t1.Errors.Count + t2.Errors.Count);

        if (t1.IsError)
        {
            errors.AddRange(t1.Errors);
        }

        if (t2.IsError)
        {
            errors.AddRange(t2.Errors);
        }

        return errors.Count > 0 ? errors : Tuple.Create(t1.Value, t2.Value);
    }

    /// <summary>
    ///     Combines a 3-tuple of errors or values into either a 3-tuple of their values when both are successful or else a
    ///     list of all their errors.
    /// </summary>
    /// <param name="tuple">A 3-tuple of <see cref="ErrorOr{T}" /> values.</param>
    /// <typeparam name="T1">The first value type of the 3-tuple.</typeparam>
    /// <typeparam name="T2">The second value type of the 3-tuple.</typeparam>
    /// <typeparam name="T3">The third value type of the 3-tuple.</typeparam>
    /// <returns>
    ///     A <see cref="Tuple{T1,T2,T3}" /> composed of the <paramref name="tuple" /> member values when all are
    ///     successful; otherwise, a list of <see cref="Error" /> values.
    /// </returns>
    public static ErrorOr<Tuple<T1, T2, T3>> Combine<T1, T2, T3>(this Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>> tuple)
    {
        var (t1, t2, t3) = tuple;

        List<Error> errors = new(t1.Errors.Count + t2.Errors.Count + t3.Errors.Count);

        if (t1.IsError)
        {
            errors.AddRange(t1.Errors);
        }

        if (t2.IsError)
        {
            errors.AddRange(t2.Errors);
        }

        if (t3.IsError)
        {
            errors.AddRange(t3.Errors);
        }

        return errors.Count > 0 ? errors : Tuple.Create(t1.Value, t2.Value, t3.Value);
    }
}
