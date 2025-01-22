using ErrorOr;

namespace Eurocentric.AdminApi.Tests.Subcutaneous.Utils;

internal static class ErrorOrExtensions
{
    internal static void ShouldNotBeError(this IErrorOr errorOr) => Assert.False(errorOr.IsError);
}
