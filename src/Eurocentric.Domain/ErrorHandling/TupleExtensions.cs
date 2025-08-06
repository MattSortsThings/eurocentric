using ErrorOr;

namespace Eurocentric.Domain.ErrorHandling;

/// <summary>
///     Extension methods for the generic tuple classes.
/// </summary>
public static class TupleExtensions
{
    /// <summary>
    ///     Combines a 2-tuple of errors or values into <i>EITHER</i> a concatenated list of all their errors when any item is
    ///     unsuccessful <i>OR</i> a 2-tuple of their values when both items are successful.
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
        (ErrorOr<T1> first, ErrorOr<T2> second) = tuple;

        if (first.IsError || second.IsError)
        {
            return first.ErrorsOrEmptyList.Concat(second.ErrorsOrEmptyList).ToList();
        }

        return Tuple.Create(first.Value, second.Value);
    }

    /// <summary>
    ///     Combines a 3-tuple of errors or values into <i>EITHER</i> a concatenated list of all their errors when any item is
    ///     unsuccessful <i>OR</i> a 3-tuple of their values when both items are successful.
    /// </summary>
    /// <param name="tuple">A 3-tuple of <see cref="ErrorOr{T}" /> values.</param>
    /// <typeparam name="T1">The first successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <typeparam name="T2">The second successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <typeparam name="T3">The thord successful value type of the <paramref name="tuple" /> argument.</typeparam>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a
    ///     <see cref="Tuple{T1,T2,T3}" /> of values.
    /// </returns>
    public static ErrorOr<Tuple<T1, T2, T3>> Combine<T1, T2, T3>(this Tuple<ErrorOr<T1>, ErrorOr<T2>, ErrorOr<T3>> tuple)
    {
        (ErrorOr<T1> first, ErrorOr<T2> second, ErrorOr<T3> third) = tuple;

        if (first.IsError || second.IsError || third.IsError)
        {
            return first.ErrorsOrEmptyList.Concat(second.ErrorsOrEmptyList).Concat(third.ErrorsOrEmptyList).ToList();
        }

        return Tuple.Create(first.Value, second.Value, third.Value);
    }
}
