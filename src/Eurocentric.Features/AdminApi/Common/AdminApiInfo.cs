using Eurocentric.Features.Shared.ApiDiscovery;
using Eurocentric.Features.Shared.Security;

namespace Eurocentric.Features.AdminApi.Common;

internal sealed class AdminApiInfo : IApiInfo
{
    internal const string ApiName = "AdminApi";

    public string Name => ApiName;

    public string UrlPrefix => "admin/api/v{version:apiVersion}";

    public IReadOnlyList<ApiRelease> Releases { get; } =
    [
        new("admin-api-v0.1", 0, 1),
        new("admin-api-v0.2", 0, 2),
        new("admin-api-v1.0", 1, 0)
    ];

    public string OpenApiDocumentTitle => "Eurocentric Admin API";

    public string OpenApiDocumentDescription => "A web API for modelling the Eurovision Song Contest, 2016-present.";

    public string AuthorizationPolicyName => nameof(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole);

    internal static class Tags
    {
        internal const string Countries = "Countries";
        internal const string Stations = "Stations";
    }
}
