using Eurocentric.Shared.ApiAbstractions;

namespace Eurocentric.AdminApi.Common;

internal sealed record ApiInfo : IApiInfo
{
    public string UrlPrefix => "admin/api/v0.1";

    public string EndpointGroupName => "admin-api";
}
