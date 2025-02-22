namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

internal static class StringExtensions
{
    internal static void ShouldNotBeEmpty(this string value) => Assert.NotEmpty(value);
}
