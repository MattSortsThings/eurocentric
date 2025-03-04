using Eurocentric.Shared.ApiAbstractions;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.AdminApi.Common;

internal sealed record ApiInfo : IApiInfo
{
    public string Id => nameof(AdminApi);

    public string UrlPrefix => "admin/api/v{version:apiVersion}";

    public string EndpointGroupName => "admin-api";

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status401Unauthorized;
            yield return StatusCodes.Status403Forbidden;
        }
    }

    public string Title => "Eurocentric Admin API";

    public string Description => "A web API for modelling the Eurovision Song Contest, 2016-present.";

    public string AuthorizationPolicyName => AuthorizationPolicyConfigurator.Name;
}
