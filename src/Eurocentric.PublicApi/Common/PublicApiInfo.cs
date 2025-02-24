using Eurocentric.Shared.ApiRegistration;

namespace Eurocentric.PublicApi.Common;

public sealed record PublicApiInfo : IApiInfo
{
    public string ApiId => "PublicApi";

    public string UrlPrefix => "public/api";

    public string EndpointGroupName => "public-api";

    public string AuthorizationPolicyName => nameof(PublicApiAuthorizationPolicy);

    public string OpenApiDocumentTitle => "Eurocentric Public API";

    public string OpenApiDocumentDescription => "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";
}
