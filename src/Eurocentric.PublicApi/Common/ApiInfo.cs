using Eurocentric.Shared.ApiAbstractions;

namespace Eurocentric.PublicApi.Common;

internal sealed record ApiInfo : IApiInfo
{
    public string UrlPrefix => "public/api/v0.1";

    public string EndpointGroupName => "public-api";
}
