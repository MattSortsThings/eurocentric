using RestSharp;

namespace Eurocentric.Tests.Utils.Fixtures;

public static class RestRequestExtensions
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public static RestRequest UseAdminApiKey(this RestRequest request) =>
        request.AddHeader(ApiKeyHeaderName, TestApiKeys.Admin);

    public static RestRequest UsePublicApiKey(this RestRequest request) =>
        request.AddHeader(ApiKeyHeaderName, TestApiKeys.Public);

    public static RestRequest UseUnrecognizedApiKey(this RestRequest request) =>
        request.AddHeader(ApiKeyHeaderName, TestApiKeys.Unrecognized);
}
