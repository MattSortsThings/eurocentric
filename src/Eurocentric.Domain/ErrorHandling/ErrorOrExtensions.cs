using ErrorOr;

namespace Eurocentric.Domain.ErrorHandling;

public static class ErrorOrExtensions
{
    public static ErrorOr<ValueTuple<T1, T2>> Combine<T1, T2>(this ValueTuple<ErrorOr<T1>, ErrorOr<T2>> tuple)
    {
        List<Error> errors = [];

        var (first, second) = tuple;

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

    public static ErrorOr<T> FailIf<T>(this ErrorOr<T> errorsOrValue, Func<T, bool> onValue, Func<T, Error> errorBuilder)
    {
        if (errorsOrValue.IsError)
        {
            return errorsOrValue;
        }

        return onValue(errorsOrValue.Value) ? errorBuilder(errorsOrValue.Value) : errorsOrValue;
    }
}
