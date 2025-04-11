using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.Shared.Versioning;

/// <summary>
///     Extension methods to be invoked at the application composition root.
/// </summary>
internal static class DependencyInjection
{
    internal static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.SubstitutionFormat = "VV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
