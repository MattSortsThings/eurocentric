using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.Tests.Unit.Utils;

internal static class ProblemDetailsExtensions
{
    internal static void ShouldHaveStatus(this ProblemDetails problemDetails, int expectedStatus) =>
        Assert.Equal(expectedStatus, problemDetails.Status);

    internal static void ShouldHaveType(this ProblemDetails problemDetails, string expectedType) =>
        Assert.Equal(expectedType, problemDetails.Type);

    internal static void ShouldHaveTitle(this ProblemDetails problemDetails, string expectedTitle) =>
        Assert.Equal(expectedTitle, problemDetails.Title);

    internal static void ShouldHaveDetail(this ProblemDetails problemDetails, string expectedDetail) =>
        Assert.Equal(expectedDetail, problemDetails.Detail);

    internal static void ShouldHaveExtension(this ProblemDetails problemDetails, string expectedKey, string expectedValue) =>
        Assert.Contains(problemDetails.Extensions,
            kvp => kvp.Key.Equals(expectedKey) && kvp.Value is string valueString && valueString == expectedValue);
}
