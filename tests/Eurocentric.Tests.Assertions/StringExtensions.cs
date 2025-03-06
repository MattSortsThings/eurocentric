namespace Eurocentric.Tests.Assertions;

public static class StringExtensions
{
    public static void ShouldEndWith(this string subject, string expectedSuffix) => Assert.EndsWith(expectedSuffix, subject);

    public static void ShouldNotBeEmpty(this string subject) => Assert.NotEmpty(subject);
}
