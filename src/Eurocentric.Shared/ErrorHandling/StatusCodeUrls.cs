namespace Eurocentric.Shared.ErrorHandling;

internal static class StatusCodeUrls
{
    internal const string Status422UnprocessableEntity = "https://tools.ietf.org/html/rfc9110#section-15.5.21";
    internal const string Status500InternalServerError = "https://tools.ietf.org/html/rfc9110#section-15.6.1";
    internal const string Status400BadRequest = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
    internal const string Status409Conflict = "https://tools.ietf.org/html/rfc9110#section-15.5.10";
    internal const string Status404NotFound = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
    internal const string Status401Unauthorized = "https://tools.ietf.org/html/rfc9110#section-15.5.2";
    internal const string Status403Forbidden = "https://tools.ietf.org/html/rfc9110#section-15.5.4";
}
