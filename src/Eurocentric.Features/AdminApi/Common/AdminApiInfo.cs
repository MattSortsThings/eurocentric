using Eurocentric.Features.Shared.ApiRegistration;

namespace Eurocentric.Features.AdminApi.Common;

internal sealed class AdminApiInfo : IApiInfo
{
    internal const string ApiName = "AdminApi";

    public string Name => ApiName;

    public string UrlPrefix => "admin/api/v0.2";

    internal static class Tags
    {
        internal const string Stations = "Stations";
    }
}
