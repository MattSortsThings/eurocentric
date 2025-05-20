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
    ///     values.
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

    /// <summary>
    ///     Combines a 3-tuple of errors or values and returns <i>EITHER</i> a 3-tuple of their values <i>OR</i> a list of all
    ///     their errors.
    /// </summary>
    /// <param name="tuple">The 2-tuple of errors or values.</param>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <returns>
    ///     A 3-tuple of two values when both elements of the tuple are successful; otherwise, a list of <see cref="Error" />
    ///     values.
    /// </returns>
    public static ErrorOr<(T1 First, T2 Second, T3 Third)> Combine<T1, T2, T3>(
        this (ErrorOr<T1> first, ErrorOr<T2> second, ErrorOr<T3> third) tuple)
    {
        var (first, second, third) = tuple;

        List<Error> errors = new(first.Errors.Count + second.Errors.Count + third.Errors.Count);

        if (first.IsError)
        {
            errors.AddRange(first.Errors);
        }

        if (second.IsError)
        {
            errors.AddRange(second.Errors);
        }

        if (third.IsError)
        {
            errors.AddRange(third.Errors);
        }

        return errors.Count > 0 ? errors : (first.Value, second.Value, third.Value);
    }

    /// <summary>
    ///     Collects a sequence of errors or values and returns <i>EITHER</i> a list of their values <i>OR</i> a list of all
    ///     their errors.
    /// </summary>
    /// <param name="errorsOrValues">A list of errors or values.</param>
    /// <typeparam name="T">The value type.</typeparam>
    /// <returns>
    ///     A list of <typeparamref name="T" /> values when all elements of the sequence are successful; otherwise, a list
    ///     of <see cref="Error" /> values.
    /// </returns>
    public static ErrorOr<List<T>> Collect<T>(this IEnumerable<ErrorOr<T>> errorsOrValues)
    {
        List<Error> errors = [];
        List<T> values = [];

        foreach (ErrorOr<T> errorsOrValue in errorsOrValues)
        {
            if (errorsOrValue.IsError)
            {
                errors.AddRange(errorsOrValue.Errors);
            }
            else if (errors.Count == 0)
            {
                values.Add(errorsOrValue.Value);
            }
        }

        return errors.Count > 0 ? errors : values;
    }
}
