using ErrorOr;

namespace Eurocentric.Domain.ErrorHandling;

/// <summary>
///     Convenience methods for the <see cref="ErrorOr" /> library.
/// </summary>
public static class ErrorHandlingExtensions
{
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

        return errors.Count > 0 ? errors : new Tuple<T1, T2>(t1.Value, t2.Value);
    }
}
