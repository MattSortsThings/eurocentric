using Eurocentric.Shared.Security;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

public static class RestRequestExtensions
{
    public static RestRequest UseAdminApiKey(this RestRequest request) =>
        request.AddHeader(ApiKeyConstants.ApiKeyHeader, TestApiKeys.Admin);

    public static RestRequest UsePublicApiKey(this RestRequest request) =>
        request.AddHeader(ApiKeyConstants.ApiKeyHeader, TestApiKeys.Public);
}
