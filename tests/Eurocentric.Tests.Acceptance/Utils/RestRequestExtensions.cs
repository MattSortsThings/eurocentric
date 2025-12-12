using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

public static class RestRequestExtensions
{
    public static RestRequest AddApiKeyHeader(this RestRequest request, string apiKey)
    {
        request.AddHeader("X-Api-Key", apiKey);

        return request;
    }
}
