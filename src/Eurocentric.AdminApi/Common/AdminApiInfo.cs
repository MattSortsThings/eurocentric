using Eurocentric.Shared.ApiRegistration;

namespace Eurocentric.AdminApi.Common;

public sealed record AdminApiInfo : IApiInfo
{
    public string ApiId => "AdminApi";

    public string UrlPrefix => "admin/api";

    public string EndpointGroupName => "admin-api";

    public string AuthorizationPolicyName => nameof(AdminApiAuthorizationPolicy);

    public string OpenApiDocumentTitle => "Eurocentric Admin API";

    public string OpenApiDocumentDescription => "A web API for modelling the Eurovision Song Contest, 2016-present.";
}
