using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Tests.Assertions;

public static class ProblemDetailsExtensions
{
    public static void ShouldHaveStatus(this ProblemDetails problemDetails, int expectedStatus) =>
        Assert.Equal(expectedStatus, problemDetails.Status);

    public static void ShouldHaveType(this ProblemDetails problemDetails, string expectedType) =>
        Assert.Equal(expectedType, problemDetails.Type);

    public static void ShouldHaveTitle(this ProblemDetails problemDetails, string expectedTitle) =>
        Assert.Equal(expectedTitle, problemDetails.Title);

    public static void ShouldHaveDetail(this ProblemDetails problemDetails, string expectedDetail) =>
        Assert.Equal(expectedDetail, problemDetails.Detail);

    public static void ShouldHaveInstance(this ProblemDetails problemDetails, string expectedInstance) =>
        Assert.Equal(expectedInstance, problemDetails.Instance);

    public static void ShouldHaveExtension(this ProblemDetails problemDetails, string expectedKey, string expectedValue) =>
        Assert.Contains(problemDetails.Extensions,
            kvp => (kvp.Key.Equals(expectedKey)
                    && kvp.Value is string valueString && valueString == expectedValue)
                   || (kvp.Value is JsonElement jsonElement && jsonElement.GetString() == expectedValue));

    public static void ShouldHaveExtension(this ProblemDetails problemDetails, string expectedKey, Guid expectedValue) =>
        Assert.Contains(problemDetails.Extensions,
            kvp => (kvp.Key.Equals(expectedKey)
                    && kvp.Value is Guid valueGuid && valueGuid == expectedValue)
                   || (kvp.Value is JsonElement jsonElement && jsonElement.GetGuid() == expectedValue));
}
