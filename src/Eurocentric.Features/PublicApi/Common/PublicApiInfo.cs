using Eurocentric.Features.Shared.ApiRegistration;

namespace Eurocentric.Features.PublicApi.Common;

internal sealed class PublicApiInfo : IApiInfo
{
    internal const string ApiName = "PublicApi";

    public string Name => ApiName;

    public string UrlPrefix => "public/api/v0.1";

    internal static class Tags
    {
        internal const string Stations = "Stations";
    }
}
