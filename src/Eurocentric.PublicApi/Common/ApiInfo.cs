using Eurocentric.Shared.ApiAbstractions;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.PublicApi.Common;

internal sealed record ApiInfo : IApiInfo
{
    public string Id => nameof(PublicApi);

    public string UrlPrefix => "public/api/v{version:apiVersion}";

    public string EndpointGroupName => "public-api";

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status401Unauthorized;
        }
    }

    public string Title => "Eurocentric Public API";

    public string Description => "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";

    public string AuthorizationPolicyName => AuthorizationPolicyConfigurator.Name;
}
