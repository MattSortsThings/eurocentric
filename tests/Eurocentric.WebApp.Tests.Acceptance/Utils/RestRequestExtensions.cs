using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

internal static class RestRequestExtensions
{
    internal static RestRequest UseAdminApiKey(this RestRequest request) => request.AddHeader("X-Api-Key", TestApiKeys.Admin);
}
