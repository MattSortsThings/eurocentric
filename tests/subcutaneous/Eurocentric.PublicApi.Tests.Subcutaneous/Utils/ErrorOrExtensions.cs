using ErrorOr;

namespace Eurocentric.PublicApi.Tests.Subcutaneous.Utils;

internal static class ErrorOrExtensions
{
    internal static void ShouldNotBeError(this IErrorOr errorOr) => Assert.False(errorOr.IsError);
}
