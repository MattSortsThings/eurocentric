using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Tests.Assertions;

public static class ProblemHttpResultExtensions
{
    public static void ShouldHaveStatusCode(this ProblemHttpResult problemHttpResult, int expectedStatusCode) =>
        Assert.Equal(expectedStatusCode, problemHttpResult.StatusCode);
}
