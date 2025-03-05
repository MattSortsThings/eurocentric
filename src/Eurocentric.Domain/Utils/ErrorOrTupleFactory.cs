using ErrorOr;

namespace Eurocentric.Domain.Utils;

internal static class ErrorOrTupleFactory
{
    internal static ErrorOr<(T1, T2)> Combine<T1, T2>(ErrorOr<T1> first, ErrorOr<T2> second)
    {
        List<Error> errors = [];

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
