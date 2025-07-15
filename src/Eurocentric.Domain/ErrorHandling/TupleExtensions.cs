using ErrorOr;

namespace Eurocentric.Domain.ErrorHandling;

/// <summary>
///     Extension methods for generic tuples using the <see cref="ErrorOr{TValue}" /> type.
/// </summary>
public static class TupleExtensions
{
    /// <summary>
    ///     Combines a 2-tuple of errors or values into <i>EITHER</i> a 2-tuple of their values when both items are successful
    ///     <i>OR ELSE</i> a concatenated list of all their errors.
    /// </summary>
    /// <param name="tuple">A 2-tuple of <see cref="ErrorOr{T}" /> values.</param>
    /// <typeparam name="T1">The first successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <typeparam name="T2">The second successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a
    ///     <see cref="Tuple{T1,T2}" /> of values.
    /// </returns>
    public static ErrorOr<Tuple<T1, T2>> Combine<T1, T2>(this Tuple<ErrorOr<T1>, ErrorOr<T2>> tuple)
    {
        var (first, second) = tuple;

        if (!first.IsError && !second.IsError)
        {
            return Tuple.Create(first.Value, second.Value);
        }

        return first.ErrorsOrEmptyList.Concat(second.ErrorsOrEmptyList).ToList();
    }

    /// <summary>
    ///     Combines a 3-tuple of errors or values into <i>EITHER</i> a 3-tuple of their values when both items are successful
    ///     <i>OR ELSE</i> a concatenated list of all their errors.
    /// </summary>
    /// <param name="tuple">A 3-tuple of <see cref="ErrorOr{T}" /> values.</param>
    /// <typeparam name="T1">The first successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <typeparam name="T2">The second successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <typeparam name="T3">The third successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a
    ///     <see cref="Tuple{T1,T2,T3}" /> of values.
    /// </returns>
    public static ErrorOr<Tuple<T1, T2, T3>> Combine<T1, T2, T3>(this Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>> tuple)
    {
        var (first, second, third) = tuple;

        if (!first.IsError && !second.IsError && !third.IsError)
        {
            return Tuple.Create(first.Value, second.Value, third.Value);
        }

        return first.ErrorsOrEmptyList.Concat(second.ErrorsOrEmptyList).Concat(third.ErrorsOrEmptyList).ToList();
    }
}
