namespace Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;

public static class StringAssertions
{
    public static void ShouldEndWith(this string subject, string expectedSuffix) => Assert.EndsWith(expectedSuffix, subject);

    public static void ShouldNotBeEmpty(this string subject) => Assert.NotEmpty(subject);
}
