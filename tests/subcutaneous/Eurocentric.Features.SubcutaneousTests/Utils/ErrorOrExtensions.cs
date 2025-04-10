using ErrorOr;

namespace Eurocentric.Features.SubcutaneousTests.Utils;

internal static class ErrorOrExtensions
{
    internal static void Deconstruct<T>(this ErrorOr<T> errorOrValue, out bool isError, out T value, out Error firstError)
    {
        isError = errorOrValue.IsError;
        value = errorOrValue.Value;
        firstError = errorOrValue.FirstError;
    }
}
