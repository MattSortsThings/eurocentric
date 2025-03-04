using Microsoft.AspNetCore.Http;

namespace Eurocentric.Shared.ErrorHandling;

internal static class HttpRequestExtensions
{
    internal static string GetInstance(this HttpRequest request) => $"{request.Method} {request.Path}{request.QueryString}";
}
