using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Shared.ErrorHandling;

/// <summary>
///     Extension methods to be invoked at application startup.
/// </summary>
internal static class Startup
{
    /// <summary>
    ///     Adds the error handling services to the application service descriptor collection.
    /// </summary>
    /// <param name="services">Contains service descriptors for the application.</param>
    /// <returns>The same <see cref="IServiceCollection" /> instance, so that method invocations can be chained.</returns>
    internal static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(config =>
        {
            config.CustomizeProblemDetails = ctx =>
            {
                HttpRequest request = ctx.HttpContext.Request;

                ctx.ProblemDetails.Instance = $"{request.Method} {request.Path}{request.QueryString}";
            };
        });

        return services;
    }
}
