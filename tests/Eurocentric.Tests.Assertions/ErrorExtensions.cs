using ErrorOr;

namespace Eurocentric.Tests.Assertions;

public static class ErrorExtensions
{
    public static void ShouldHaveErrorType(this Error subject, ErrorType expectedType) =>
        Assert.Equal(expectedType, subject.Type);

    public static void ShouldHaveCode(this Error subject, string expectedCode) =>
        Assert.Equal(expectedCode, subject.Code);

    public static void ShouldHaveDescription(this Error subject, string expectedDescription) =>
        Assert.Equal(expectedDescription, subject.Description);

    public static void ShouldHaveMetadata(this Error subject, string expectedKey, string expectedValue)
    {
        Assert.NotNull(subject.Metadata);
        Assert.True(subject.Metadata.TryGetValue(expectedKey, out object? value) && value is string s && s == expectedValue);
    }

    public static void ShouldHaveMetadata(this Error subject, string expectedKey, Guid expectedValue)
    {
        Assert.NotNull(subject.Metadata);
        Assert.True(subject.Metadata.TryGetValue(expectedKey, out object? value) && value is Guid g && g == expectedValue);
    }
}
