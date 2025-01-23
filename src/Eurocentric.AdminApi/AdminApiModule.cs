using Eurocentric.Shared.ApiModules;

namespace Eurocentric.AdminApi;

public sealed class AdminApiModule : ApiModule
{
    public override string ApiName => "admin-api";

    public override string Prefix => "admin/api";
}
