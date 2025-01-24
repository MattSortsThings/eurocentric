using System.Net;

namespace Eurocentric.Shared.Tests.Acceptance.Utils;

internal static class HttpStatusCodeExtensions
{
    internal static void ShouldBe(this HttpStatusCode httpStatusCode, HttpStatusCode expected) =>
        Assert.Equal(expected, httpStatusCode);
}
