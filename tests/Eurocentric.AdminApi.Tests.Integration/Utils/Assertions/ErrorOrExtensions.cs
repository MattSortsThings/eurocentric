using ErrorOr;

namespace Eurocentric.AdminApi.Tests.Integration.Utils.Assertions;

public static class ErrorOrExtensions
{
    public static void ShouldNotBeError(this IErrorOr errorOr) => Assert.False(errorOr.IsError);
}
