using Eurocentric.Shared.ApiModules;

namespace Eurocentric.AdminApi.Common;

internal sealed class AdminApiModule : ApiModule
{
    protected override string ApiName => "AdminApi";

    protected override string UrlPrefix => "admin/api/v{version:apiVersion}";

    protected override string EndpointGroupName => "admin-api";

    protected override string? AuthorizationPolicyName => nameof(AdminApiAuthorizationPolicy);

    protected override string OpenApiDocumentTitle => "Eurocentric Admin API";

    protected override string OpenApiDocumentDescription => "A web API for modelling the Eurovision Song Contest, 2016-present.";
}
