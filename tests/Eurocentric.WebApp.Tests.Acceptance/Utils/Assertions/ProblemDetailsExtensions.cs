using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;

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

    internal static void ShouldHaveInstance(this ProblemDetails problemDetails, string expectedInstance) =>
        Assert.Equal(expectedInstance, problemDetails.Instance);

    internal static void ShouldHaveExtension(this ProblemDetails problemDetails, string expectedKey, Guid expectedValue) =>
        Assert.Contains(problemDetails.Extensions,
            kvp => kvp.Key.Equals(expectedKey)
                   && kvp.Value is JsonElement jsonElement
                   && jsonElement.GetGuid() == expectedValue);

    internal static void ShouldHaveExtension(this ProblemDetails problemDetails, string expectedKey, string expectedValue) =>
        Assert.Contains(problemDetails.Extensions,
            kvp => kvp.Key.Equals(expectedKey)
                   && kvp.Value is JsonElement jsonElement
                   && jsonElement.GetString() == expectedValue);
}
