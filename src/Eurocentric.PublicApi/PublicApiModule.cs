using Eurocentric.Shared.ApiModules;
using Eurocentric.Shared.Security;

namespace Eurocentric.PublicApi;

public class PublicApiModule : ApiModule
{
    public override string ApiName => "public-api";

    public override string Prefix => "public/api";

    protected override string? AuthorizationPolicyName => nameof(AuthorizationPolicies.AuthenticatedAdminOrUserOnly);
}
