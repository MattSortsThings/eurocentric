using Eurocentric.Shared.ApiAbstractions;

namespace Eurocentric.PublicApi.Common;

internal sealed record ApiInfo : IApiInfo
{
    public string Id => nameof(PublicApi);

    public string UrlPrefix => "public/api/v{version:apiVersion}";

    public string EndpointGroupName => "public-api";
}
