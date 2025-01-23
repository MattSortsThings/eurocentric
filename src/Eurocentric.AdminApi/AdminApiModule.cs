using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.Security;

namespace Eurocentric.AdminApi;

public sealed class AdminApiModule : ApiModule
{
    public override string ApiName => "admin-api";

    public override string Prefix => "admin/api";

    protected override string? AuthorizationPolicyName => nameof(AuthorizationPolicies.AuthenticatedAdminOnly);
}
