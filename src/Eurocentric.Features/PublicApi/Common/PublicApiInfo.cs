using Eurocentric.Features.Shared.ApiDiscovery;

namespace Eurocentric.Features.PublicApi.Common;

internal sealed class PublicApiInfo : IApiInfo
{
    internal const string ApiName = "PublicApi";

    public string Name => ApiName;

    public string UrlPrefix => "public/api/v{version:apiVersion}";

    public IReadOnlyList<ApiRelease> Releases { get; } =
    [
        new("public-api-v0.1", 0, 1)
    ];

    public string OpenApiDocumentTitle => "Eurocentric Public API";

    public string OpenApiDocumentDescription => "A web API for (over)analysing the Eurovision Song Contest, 2016-present.";

    internal static class Tags
    {
        internal const string Stations = "Stations";
    }
}
