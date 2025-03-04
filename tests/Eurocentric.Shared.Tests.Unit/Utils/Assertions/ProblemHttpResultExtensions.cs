using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Shared.Tests.Unit.Utils.Assertions;

internal static class ProblemHttpResultExtensions
{
    internal static void ShouldHaveStatusCode(this ProblemHttpResult problemHttpResult, int expectedStatusCode) =>
        Assert.Equal(expectedStatusCode, problemHttpResult.StatusCode);
}
