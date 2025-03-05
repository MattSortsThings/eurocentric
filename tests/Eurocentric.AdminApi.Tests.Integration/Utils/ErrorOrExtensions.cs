using ErrorOr;

namespace Eurocentric.AdminApi.Tests.Integration.Utils;

public static class ErrorOrExtensions
{
    public static (bool IsError, T Value) ParseAsSuccess<T>(this ErrorOr<T> errorOr) =>
        new(errorOr.IsError, errorOr.Value);

    public static (bool IsError, Error FirstError) ParseAsError<T>(this ErrorOr<T> errorOr) =>
        new(errorOr.IsError, errorOr.FirstError);
}
