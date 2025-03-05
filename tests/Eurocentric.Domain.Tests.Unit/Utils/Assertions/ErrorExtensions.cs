using ErrorOr;

namespace Eurocentric.Domain.Tests.Unit.Utils.Assertions;

public static class ErrorExtensions
{
    public static void ShouldHaveFailureErrorType(this Error subject) => Assert.Equal(ErrorType.Failure, subject.Type);

    public static void ShouldHaveCode(this Error subject, string expectedCode) => Assert.Equal(expectedCode, subject.Code);

    public static void ShouldHaveDescription(this Error subject, string expectedDescription) =>
        Assert.Equal(expectedDescription, subject.Description);

    public static void ShouldHaveMetadata(this Error subject, string expectedKey, string expectedValue)
    {
        Assert.NotNull(subject.Metadata);
        Assert.True(subject.Metadata.TryGetValue(expectedKey, out object? value) && value is string s && s == expectedValue);
    }
}
