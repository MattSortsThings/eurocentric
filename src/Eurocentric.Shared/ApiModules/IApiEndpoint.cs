using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiModules;

public interface IApiEndpoint
{
    public string EndpointName { get; }

    public ApiVersion InitialApiVersion { get; }

    public RouteHandlerBuilder Map(IEndpointRouteBuilder apiGroup);
}
