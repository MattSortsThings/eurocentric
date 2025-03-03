using ErrorOr;

namespace Eurocentric.PublicApi.Tests.Integration.Utils.Assertions;

public static class ErrorOrExtensions
{
    public static void ShouldNotBeError(this IErrorOr errorOr) => Assert.False(errorOr.IsError);
}
