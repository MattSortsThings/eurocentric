using Eurocentric.Shared.OpenApi.Transformers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ApiModules;

public abstract class ApiModule : IApiModule
{
    protected virtual string? AuthorizationPolicyName => null;

    public abstract string ApiName { get; }

    public abstract string Prefix { get; }

    public void MapVersionedApiEndpoints(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiName).MapGroup(Prefix);

        if (AuthorizationPolicyName != null)
        {
            apiGroup.RequireAuthorization(AuthorizationPolicyName);
        }
        else
        {
            apiGroup.AllowAnonymous();
        }

        foreach (ApiRelease apiRelease in GetAllEndpointsInAssembly().MapToApiReleases(ApiName))
        {
            apiGroup.MapApiRelease(apiRelease);
        }
    }

    public void AddOpenApiDocuments(IServiceCollection services)
    {
        foreach (string name in GetOpenApiDocumentNames())
        {
            services.AddOpenApi(name, options =>
            {
                options.AddDocumentTransformer<ApiKeySecuritySchemeTransformer>();
            });
        }
    }

    private IApiEndpoint[] GetAllEndpointsInAssembly() =>
        GetType().Assembly.GetTypes()
            .Where(type => typeof(IApiEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Select(type => (IApiEndpoint)Activator.CreateInstance(type)!)
            .ToArray();

    private IEnumerable<string> GetOpenApiDocumentNames() =>
        GetAllEndpointsInAssembly().GetDistinctApiVersions().Select(version => version.MapToApiReleaseGroupName(ApiName));
}
