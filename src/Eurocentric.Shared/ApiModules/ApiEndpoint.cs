using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiModules;

public abstract class ApiEndpoint : IApiEndpoint
{
    public abstract int MajorVersion { get; }

    public abstract int MinorVersion { get; }

    public virtual string DisplayName => GetType().Name.Replace("Endpoint", string.Empty);

    public abstract RouteHandlerBuilder Map(IEndpointRouteBuilder releaseGroup);
}
