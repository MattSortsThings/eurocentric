using ErrorOr;

namespace Eurocentric.Domain.ErrorHandling;

/// <summary>
///     Extension methods for generic lists using the <see cref="ErrorOr{TValue}" /> type.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    ///     Collects a list of errors or values into <i>EITHER</i> a list of the all values when all items are successful
    ///     <i>OR ELSE</i> a flattened list of all the errors.
    /// </summary>
    /// <remarks>The ordering of list items is preserved.</remarks>
    /// <param name="items">A list of <see cref="ErrorOr{T}" /> values.</param>
    /// <typeparam name="T">The successful value type.</typeparam>
    /// <returns>
    ///     The discriminated union of <i>EITHER</i> a list of <see cref="Error" /> objects <i>OR</i> a list of type
    ///     <typeparamref name="T" />.
    /// </returns>
    public static ErrorOr<List<T>> Collect<T>(this List<ErrorOr<T>> items) => items.Any(item => item.IsError)
        ? items.SelectMany(item => item.ErrorsOrEmptyList).ToList()
        : items.Select(item => item.Value).ToList();
}
