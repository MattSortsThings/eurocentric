using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Shared.Tests.Unit.ErrorHandling.Utils;

internal static class ProblemHttpResultExtensions
{
    internal static void ShouldHaveStatusCode(this ProblemHttpResult result, int statusCode) =>
        Assert.Equal(result.StatusCode, statusCode);
}
