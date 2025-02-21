using Eurocentric.Shared.ApiModules;

namespace Eurocentric.AdminApi.Common;

internal sealed class AdminApiModule : ApiModule
{
    protected override string ApiName => "AdminApi";

    protected override string UrlPrefix => "admin/api/v{version:apiVersion}";

    protected override string EndpointGroupName => "admin-api";
}
