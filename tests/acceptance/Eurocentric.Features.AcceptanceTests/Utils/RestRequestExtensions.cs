using Eurocentric.TestUtils.WebAppFixtures;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

internal static class RestRequestExtensions
{
    internal static RestRequest UseAdminApiKey(this RestRequest request) => request.AddHeader("X-Api-Key", TestApiKeys.Admin);

    internal static RestRequest UsePublicApiKey(this RestRequest request) => request.AddHeader("X-Api-Key", TestApiKeys.Public);
}
