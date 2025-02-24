using Asp.Versioning;
using Eurocentric.Shared.ApiRegistration;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.AdminApi.Common;

public sealed record AdminApiInfo : IApiInfo
{
    internal static readonly int[] UniversalProblemStatusCodes =
    [
        StatusCodes.Status401Unauthorized,
        StatusCodes.Status403Forbidden
    ];

    public string ApiId => "AdminApi";

    public string UrlPrefix => "admin/api";

    public string EndpointGroupName => "admin-api";

    public string AuthorizationPolicyName => nameof(AdminApiAuthorizationPolicy);

    public string OpenApiDocumentTitle => "Eurocentric Admin API";

    public string OpenApiDocumentDescription => "A web API for modelling the Eurovision Song Contest, 2016-present.";

    internal static class Versions
    {
        internal static class V0
        {
            internal static readonly ApiVersion Point1 = new(0, 1);
            internal static readonly ApiVersion Point2 = new(0, 2);
        }
    }

    internal static class Tags
    {
        internal const string Calculations = "Calculations";
    }
}
