using Asp.Versioning;
using Eurocentric.Shared.ApiRegistration;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.PublicApi.Common;

public sealed record PublicApiInfo : IApiInfo
{
    internal static readonly int[] UniversalProblemStatusCodes = [StatusCodes.Status401Unauthorized];

    public string ApiId => "PublicApi";

    public string UrlPrefix => "public/api";

    public string EndpointGroupName => "public-api";

    public string AuthorizationPolicyName => nameof(PublicApiAuthorizationPolicy);

    public string OpenApiDocumentTitle => "Eurocentric Public API";

    public string OpenApiDocumentDescription => "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";

    internal static class Tags
    {
        internal const string Stations = "Stations";
    }

    internal static class Versions
    {
        internal static class V0
        {
            internal static readonly ApiVersion Point1 = new(0, 1);
        }
    }
}
