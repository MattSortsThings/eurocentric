using System.Net;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;

public static class HttpStatusCodeExtensions
{
    public static void ShouldBe(this HttpStatusCode subject, HttpStatusCode expected) => Assert.Equal(expected, subject);
}
