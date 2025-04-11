using Eurocentric.Features.Shared.ApiDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.Shared.EndpointMapping;

internal static class Configuration
{
    internal static void UseVersionedApiEndpoints(this IEndpointRouteBuilder app)
    {
        using IServiceScope scope = app.ServiceProvider.CreateScope();

        IApiInfo[] apis = scope.ServiceProvider.GetRequiredService<IEnumerable<IApiInfo>>().ToArray();
        IEndpointInfo[] endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpointInfo>>().ToArray();

        foreach (IApiInfo api in apis)
        {
            RouteGroupBuilder apiGroup = app.NewVersionedApi(api.Name)
                .MapGroup(api.UrlPrefix)
                .WithGroupName(api.Name)
                .AllowAnonymous();

            foreach (IEndpointInfo endpoint in endpoints.Where(endpoint => endpoint.ApiName == api.Name))
            {
                RouteHandlerBuilder routeBuilder = apiGroup.MapEndpoint(endpoint.HttpMethod, endpoint.Route, endpoint.Handler)
                    .WithName(endpoint.Name)
                    .WithSummary(endpoint.Summary)
                    .WithDescription(endpoint.Description)
                    .WithTags(endpoint.Tag)
                    .ProducesProblems(endpoint.ProblemStatusCodes);

                foreach (var (_, major, minor) in api.Releases)
                {
                    if (endpoint.MajorApiVersion == major && endpoint.MinorApiVersion <= minor)
                    {
                        routeBuilder.HasApiVersion(major, minor);
                    }
                }
            }
        }
    }
}
