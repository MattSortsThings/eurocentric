using ErrorOr;

namespace Eurocentric.Domain.ErrorHandling;

/// <summary>
///     Extension methods for the <see cref="ErrorOr" /> types.
/// </summary>
public static class ErrorOrExtensions
{
    /// <summary>
    ///     Combines a 2-tuple of errors or values and returns <i>EITHER</i> a 2-tuple of their values <i>OR</i> a list of all
    ///     their errors.
    /// </summary>
    /// <param name="tuple">The 2-tuple of errors or values.</param>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <returns>
    ///     A 2-tuple of two values when both elements of the tuple are successful; otherwise, a list of <see cref="Error" />
    ///     objects.
    /// </returns>
    public static ErrorOr<(T1 First, T2 Second)> Combine<T1, T2>(this (ErrorOr<T1> first, ErrorOr<T2> second) tuple)
    {
        var (first, second) = tuple;

        List<Error> errors = new(first.Errors.Count + second.Errors.Count);
        if (first.IsError)
        {
            errors.AddRange(first.Errors);
        }

        if (second.IsError)
        {
            errors.AddRange(second.Errors);
        }

        return errors.Count > 0 ? errors : (first.Value, second.Value);
    }
}
