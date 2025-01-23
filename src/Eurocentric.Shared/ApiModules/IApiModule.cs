using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiModules;

public interface IApiModule
{
    public string ApiName { get; }

    public string Prefix { get; }

    public void MapVersionedApiEndpoints(IEndpointRouteBuilder app);
}
