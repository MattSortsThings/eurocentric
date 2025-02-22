using Eurocentric.Shared.ApiModules;

namespace Eurocentric.PublicApi.Common;

internal sealed class PublicApiModule : ApiModule
{
    protected override string ApiName => "PublicApi";

    protected override string UrlPrefix => "public/api/v{version:apiVersion}";

    protected override string EndpointGroupName => "public-api";

    protected override string? AuthorizationPolicyName => nameof(PublicApiAuthorizationPolicy);

    protected override string OpenApiDocumentTitle => "Eurocentric Public API";

    protected override string OpenApiDocumentDescription =>
        "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";
}
