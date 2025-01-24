using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.Tests.Acceptance.Utils;

internal static class ProblemDetailsExtensions
{
    internal static void ShouldHaveStatus(this ProblemDetails problemDetails, int status) =>
        Assert.Equal(status, problemDetails.Status);

    internal static void ShouldHaveTitle(this ProblemDetails problemDetails, string title) =>
        Assert.Equal(title, problemDetails.Title);

    internal static void ShouldHaveDetail(this ProblemDetails problemDetails, string detail) =>
        Assert.Equal(detail, problemDetails.Detail);

    internal static void ShouldHaveInstance(this ProblemDetails problemDetails, string instance) =>
        Assert.Equal(instance, problemDetails.Instance);

    internal static void ShouldHaveType(this ProblemDetails problemDetails, string type) =>
        Assert.Equal(type, problemDetails.Type);

    internal static void ShouldHaveExtensionsEntry(this ProblemDetails problemDetails, string key, int value) =>
        Assert.Contains(problemDetails.Extensions, pair => pair.Key == key
                                                           && pair.Value is not null
                                                           && pair.Value.ToString() == value.ToString());

    internal static void ShouldHaveExtensionsEntry(this ProblemDetails problemDetails, string key, string value) =>
        Assert.Contains(problemDetails.Extensions, pair => pair.Key == key
                                                           && pair.Value is not null
                                                           && pair.Value.ToString() == value);

    internal static void ShouldHaveEmptyExtensions(this ProblemDetails problemDetails) =>
        Assert.Empty(problemDetails.Extensions);

    internal static void ShouldHaveSingleExtension(this ProblemDetails problemDetails, string key) =>
        Assert.True(problemDetails.Extensions.Count == 1 && problemDetails.Extensions.ContainsKey(key));
}
