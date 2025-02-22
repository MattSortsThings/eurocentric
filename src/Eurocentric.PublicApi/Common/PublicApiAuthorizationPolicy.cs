using Eurocentric.Shared.Security;
using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.PublicApi.Common;

internal sealed class PublicApiAuthorizationPolicy : IApiAuthorizationPolicy
{
    public void Configure(AuthorizationPolicyBuilder policyBuilder) =>
        policyBuilder.RequireAuthenticatedUser().RequireRole(Roles.User);
}
