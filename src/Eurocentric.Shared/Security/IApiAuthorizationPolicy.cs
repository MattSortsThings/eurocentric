using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.Shared.Security;

/// <summary>
///     Describes an API authorization policy.
/// </summary>
/// <remarks>Register an implementation of this interface as a transient service in each API project.</remarks>
public interface IApiAuthorizationPolicy
{
    public void Configure(AuthorizationPolicyBuilder policyBuilder);
}
