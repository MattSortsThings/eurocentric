using Eurocentric.Shared.ApiModules;

namespace Eurocentric.PublicApi.Common;

internal sealed class PublicApiModule : ApiModule
{
    protected override string ApiName => "PublicApi";

    protected override string UrlPrefix => "public/api/v{version:apiVersion}";

    protected override string EndpointGroupName => "public-api";
}
