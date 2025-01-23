using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ApiModules;

public interface IApiModule
{
    public string ApiName { get; }

    public string Prefix { get; }

    public void MapVersionedApiEndpoints(IEndpointRouteBuilder app);

    public void AddOpenApiDocuments(IServiceCollection services);
}
