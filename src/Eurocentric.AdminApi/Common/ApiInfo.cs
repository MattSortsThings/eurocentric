using Eurocentric.Shared.ApiAbstractions;

namespace Eurocentric.AdminApi.Common;

internal sealed record ApiInfo : IApiInfo
{
    public string Id => nameof(AdminApi);

    public string UrlPrefix => "admin/api/v{version:apiVersion}";

    public string EndpointGroupName => "admin-api";
}
